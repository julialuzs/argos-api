using System.Text.Json;
using ArgosApi.Data;
using ArgosApi.Domain.Entities;
using ArgosApi.Domain.Enums;
using ArgosApi.Features.Relatorios.Auditoria;
using ArgosApi.Features.Relatorios.Helpers;
using ArgosApi.Features.Relatorios.Requests;
using ArgosApi.Features.Relatorios.Responses;
using ArgosApi.Features.Usuarios;
using Microsoft.EntityFrameworkCore;

namespace ArgosApi.Features.Relatorios
{
    /// <summary>
    /// Resultado da tentativa de iniciar uma auditoria sob demanda
    /// </summary>
    public record IniciarAuditoriaResultado(int StatusCode, string? Message);

    /// <summary>
    /// Service responsável por gerenciar os relatórios
    /// </summary>
    public class RelatoriosService(AppDbContext context, CurrentUser currentUser, AuditoriaQueue auditoriaQueue)
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
        /// <summary>
        /// Busca relatório pelo id
        /// </summary>
        public async Task<RelatorioDetalheResponse?> GetRelatorioPorId(long id, CancellationToken cancellationToken)
        {
            var relatorio = await context.Relatorios.FindAsync(id, cancellationToken);
            if (relatorio is null)
            {
                return null;
            }

            return RelatorioAuditoriaMapper.MapearParaDetalhe(relatorio);
        }

        /// <summary>
        /// Busca todos os relatorios pelo id do projeto
        /// </summary>
        public async Task<IEnumerable<Relatorio>> ListarRelatoriosPorProjeto(long idProjeto, CancellationToken cancellationToken)
        {
            return context.Relatorios
                .Where((relatorio) => relatorio.ProjetoId == idProjeto)
                .OrderByDescending(r => r.DataHoraExecucao);
        }

        /// <summary>
        /// Salvar relatorio na base de dados
        /// </summary>
        public async Task SalvarRelatorio(RelatorioRequest request, CancellationToken cancellationToken)
        {
            var jsonText = request.Json.GetRawText();
            RelatorioAuditoriaJson auditoria;

            try
            {
                auditoria = JsonSerializer.Deserialize<RelatorioAuditoriaJson>(jsonText, JsonOptions)
                    ?? throw new JsonException("JSON do relatório é nulo.");
            }
            catch (JsonException ex)
            {
                throw new RelatorioJsonInvalidoException(
                    $"O JSON do relatório não pôde ser processado. Path: {ex.Path ?? "(desconhecido)"}. Detalhe: {ex.Message}",
                    ex);
            }

            var relatorio = new Relatorio
            {
                Json = jsonText,
                ProjetoId = request.IdProjeto,
                DataHoraExecucao = auditoria?.AuditDate ?? DateTime.UtcNow,
                Pontuacao = auditoria?.Summary?.Score ?? 0,
                TradutorLibrasIdentificado = auditoria?.Summary.AssistiveTechnologies?.VLibras ?? false,
                QuantidadeErros = RelatorioAuditoriaCalculator.ContarApontamentosPorSeveridade(auditoria, SeveridadeEnum.Serious, SeveridadeEnum.Critical),
                QuantidadeAvisos = RelatorioAuditoriaCalculator.ContarApontamentosPorSeveridade(auditoria, SeveridadeEnum.Moderate, SeveridadeEnum.Minor)
            };

            var projeto = await context.Projetos.FindAsync([request.IdProjeto], cancellationToken);
            projeto!.UltimaExecucao = relatorio.DataHoraExecucao;
            projeto.StatusExecucao = StatusExecucao.Idle;
            projeto.MensagemErroExecucao = null;

            await context.Relatorios.AddAsync(relatorio, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Enfileira a execução sob demanda do avaliador para o projeto
        /// </summary>
        public async Task<IniciarAuditoriaResultado> IniciarAuditoria(long idProjeto, CancellationToken cancellationToken)
        {
            var projeto = await context.Projetos
                .AsNoTracking()
                .Where(p => p.Id == idProjeto && p.Usuarios.Any(u => u.Id == currentUser.Id))
                .FirstOrDefaultAsync(cancellationToken);

            if (projeto is null)
            {
                return new IniciarAuditoriaResultado(StatusCodes.Status404NotFound, null);
            }

            if (string.IsNullOrWhiteSpace(projeto.UrlBase))
            {
                return new IniciarAuditoriaResultado(
                    StatusCodes.Status400BadRequest,
                    "Informe a URL base do projeto antes de executar a análise.");
            }

            if (projeto.StatusExecucao == StatusExecucao.Executando)
            {
                return new IniciarAuditoriaResultado(
                    StatusCodes.Status409Conflict,
                    "Já existe uma análise em andamento para este projeto.");
            }

            var atualizados = await context.Projetos
                .Where(p => p.Id == idProjeto && p.StatusExecucao != StatusExecucao.Executando)
                .ExecuteUpdateAsync(
                    s => s
                        .SetProperty(p => p.StatusExecucao, StatusExecucao.Executando)
                        .SetProperty(p => p.MensagemErroExecucao, (string?)null),
                    cancellationToken);

            if (atualizados == 0)
            {
                return new IniciarAuditoriaResultado(
                    StatusCodes.Status409Conflict,
                    "Já existe uma análise em andamento para este projeto.");
            }

            await auditoriaQueue.EnqueueAsync(idProjeto, cancellationToken);
            return new IniciarAuditoriaResultado(StatusCodes.Status202Accepted, null);
        }

    }
}