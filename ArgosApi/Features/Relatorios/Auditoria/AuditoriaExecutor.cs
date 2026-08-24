using System.Diagnostics;
using System.Text;
using System.Text.Json;
using ArgosApi.Data;
using ArgosApi.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ArgosApi.Features.Relatorios.Auditoria
{
    public class AuditoriaExecutor(
        IServiceScopeFactory scopeFactory,
        IWebHostEnvironment environment,
        IOptions<ArgosAvaliadorOptions> options,
        ILogger<AuditoriaExecutor> logger)
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public async Task ExecutarAsync(long projetoId, CancellationToken cancellationToken)
        {
            var opts = options.Value;
            var cliPath = ResolverCaminho(opts.CliEntryPoint);
            var workingDirectory = ResolverCaminho(opts.WorkingDirectory);

            await using var scope = scopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var projeto = await context.Projetos.FirstOrDefaultAsync(p => p.Id == projetoId, cancellationToken);
            if (projeto is null)
            {
                logger.LogWarning("Projeto {ProjetoId} não encontrado para auditoria", projetoId);
                return;
            }

            if (string.IsNullOrWhiteSpace(cliPath) || !File.Exists(cliPath))
            {
                await MarcarFalhaAsync(context, projetoId, $"CLI do avaliador não encontrado em '{cliPath}'.", cancellationToken);
                return;
            }

            if (string.IsNullOrWhiteSpace(workingDirectory) || !Directory.Exists(workingDirectory))
            {
                await MarcarFalhaAsync(context, projetoId, $"Diretório de trabalho do avaliador inválido: '{workingDirectory}'.", cancellationToken);
                return;
            }

            var tempDir = Path.Combine(Path.GetTempPath(), "argos-auditoria", $"{projetoId}-{Guid.NewGuid():N}");
            Directory.CreateDirectory(tempDir);
            var configPath = Path.Combine(tempDir, "argos.config.json");
            var outputPath = Path.Combine(tempDir, "report.json");

            var config = new
            {
                baseUrl = projeto.UrlBase,
                routes = projeto.Rotas is { Length: > 0 } ? projeto.Rotas : new[] { "/" },
                includeW3c = projeto.IncluirW3c,
                projectId = projeto.Id
            };

            await File.WriteAllTextAsync(configPath, JsonSerializer.Serialize(config, JsonOptions), cancellationToken);

            var psi = new ProcessStartInfo
            {
                FileName = string.IsNullOrWhiteSpace(opts.NodePath) ? "node" : opts.NodePath,
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            psi.ArgumentList.Add(cliPath);
            psi.ArgumentList.Add("--config");
            psi.ArgumentList.Add(configPath);
            psi.ArgumentList.Add("--out");
            psi.ArgumentList.Add(outputPath);
            psi.Environment["ARGOS_API_RELATORIOS_ENDPOINT"] = opts.ApiRelatoriosEndpoint;

            logger.LogInformation("Iniciando avaliador Argos para o projeto {ProjetoId}", projetoId);

            using var process = new Process { StartInfo = psi, EnableRaisingEvents = true };
            var stdOut = new StringBuilder();
            var stdErr = new StringBuilder();
            process.OutputDataReceived += (_, e) => { if (e.Data is not null) stdOut.AppendLine(e.Data); };
            process.ErrorDataReceived += (_, e) => { if (e.Data is not null) stdErr.AppendLine(e.Data); };

            try
            {
                if (!process.Start())
                {
                    await MarcarFalhaAsync(context, projetoId, "Não foi possível iniciar o processo Node do avaliador.", cancellationToken);
                    return;
                }

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                timeoutCts.CancelAfter(TimeSpan.FromMinutes(Math.Max(1, opts.TimeoutMinutes)));

                try
                {
                    await process.WaitForExitAsync(timeoutCts.Token);
                }
                catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
                {
                    EncerrarProcesso(process);
                    await MarcarFalhaAsync(context, projetoId, $"A análise excedeu o tempo limite de {opts.TimeoutMinutes} minutos.", CancellationToken.None);
                    return;
                }

                if (process.ExitCode != 0)
                {
                    var detalhe = Truncar(stdErr.Length > 0 ? stdErr.ToString() : stdOut.ToString());
                    await MarcarFalhaAsync(
                        context,
                        projetoId,
                        string.IsNullOrWhiteSpace(detalhe)
                            ? $"O avaliador encerrou com código {process.ExitCode}."
                            : $"O avaliador encerrou com código {process.ExitCode}: {detalhe}",
                        cancellationToken);
                    return;
                }

                await MarcarIdleSeAindaExecutandoAsync(context, projetoId, cancellationToken);
                logger.LogInformation("Avaliador Argos concluído para o projeto {ProjetoId}", projetoId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao executar avaliador para o projeto {ProjetoId}", projetoId);
                await MarcarFalhaAsync(context, projetoId, Truncar(ex.Message), CancellationToken.None);
            }
            finally
            {
                try
                {
                    Directory.Delete(tempDir, recursive: true);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Não foi possível remover o diretório temporário {TempDir}", tempDir);
                }
            }
        }

        private string ResolverCaminho(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return path;
            }

            return Path.IsPathRooted(path)
                ? path
                : Path.GetFullPath(Path.Combine(environment.ContentRootPath, path));
        }

        private static void EncerrarProcesso(Process process)
        {
            try
            {
                if (!process.HasExited)
                {
                    process.Kill(entireProcessTree: true);
                    process.WaitForExit(5000);
                }
            }
            catch
            {
                // ignored
            }
        }

        private static async Task MarcarFalhaAsync(AppDbContext context, long projetoId, string mensagem, CancellationToken cancellationToken)
        {
            var projeto = await context.Projetos.FirstOrDefaultAsync(p => p.Id == projetoId, cancellationToken);
            if (projeto is null || projeto.StatusExecucao != StatusExecucao.Executando)
            {
                return;
            }

            projeto.StatusExecucao = StatusExecucao.Falhou;
            projeto.MensagemErroExecucao = Truncar(mensagem);
            await context.SaveChangesAsync(cancellationToken);
        }

        private static async Task MarcarIdleSeAindaExecutandoAsync(AppDbContext context, long projetoId, CancellationToken cancellationToken)
        {
            var projeto = await context.Projetos.FirstOrDefaultAsync(p => p.Id == projetoId, cancellationToken);
            if (projeto is null || projeto.StatusExecucao != StatusExecucao.Executando)
            {
                return;
            }

            projeto.StatusExecucao = StatusExecucao.Idle;
            projeto.MensagemErroExecucao = null;
            await context.SaveChangesAsync(cancellationToken);
        }

        private static string Truncar(string texto, int max = 1800)
        {
            var trimmed = texto.Trim();
            return trimmed.Length <= max ? trimmed : trimmed[^max..];
        }
    }
}
