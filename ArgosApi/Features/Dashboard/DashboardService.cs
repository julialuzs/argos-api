using System.Text.Json;
using ArgosApi.Data;
using ArgosApi.Features.Relatorios;
using ArgosApi.Features.Relatorios.Helpers;
using ArgosApi.Features.Relatorios.Requests;
using ArgosApi.Features.Usuarios;
using Microsoft.EntityFrameworkCore;

namespace ArgosApi.Features.Dashboard
{
    /// <summary>
    /// Service responsável por agregar os dados da tela de dashboard
    /// </summary>
    public class DashboardService(AppDbContext context, CurrentUser currentUser)
    {
        private const int LimiteExecucoes = 12;
        private const int LimiteCriteriosEmag = 10;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Monta os dados do dashboard do projeto. Retorna nulo se o projeto não existir ou não pertencer ao usuário.
        /// </summary>
        public async Task<DashboardResponse?> GetDashboard(long idProjeto, CancellationToken cancellationToken)
        {
            var projetoExiste = await context.Projetos
                .AsNoTracking()
                .AnyAsync(p => p.Id == idProjeto && p.Usuarios.Any(u => u.Id == currentUser.Id), cancellationToken);

            if (!projetoExiste)
            {
                return null;
            }

            var series = await context.Relatorios
                .AsNoTracking()
                .Where(r => r.ProjetoId == idProjeto)
                .OrderByDescending(r => r.DataHoraExecucao)
                .Take(LimiteExecucoes)
                .Select(r => new DashboardSerieExecucaoResponse
                {
                    DataHoraExecucao = r.DataHoraExecucao,
                    Pontuacao = r.Pontuacao,
                    QuantidadeErros = r.QuantidadeErros,
                    QuantidadeAvisos = r.QuantidadeAvisos
                })
                .ToListAsync(cancellationToken);

            series.Reverse();

            var response = new DashboardResponse { Series = series };
            if (series.Count == 0)
            {
                return response;
            }

            var ultimoRelatorio = await context.Relatorios
                .AsNoTracking()
                .Where(r => r.ProjetoId == idProjeto)
                .OrderByDescending(r => r.DataHoraExecucao)
                .Select(r => new { r.Json, r.TradutorLibrasIdentificado })
                .FirstOrDefaultAsync(cancellationToken);

            var ultima = series[^1];
            var auditoria = DesserializarAuditoria(ultimoRelatorio?.Json);

            response.Resumo = new DashboardResumoResponse
            {
                DataHoraExecucao = ultima.DataHoraExecucao,
                Pontuacao = ultima.Pontuacao,
                VariacaoPontuacao = series.Count >= 2 ? ultima.Pontuacao - series[^2].Pontuacao : null,
                QuantidadeErros = ultima.QuantidadeErros,
                QuantidadeAvisos = ultima.QuantidadeAvisos,
                RotasAuditadas = auditoria?.Summary.RoutesAudited
                    ?? auditoria?.Results.Count
                    ?? 0,
                TradutorLibrasIdentificado = auditoria?.Summary.AssistiveTechnologies?.VLibras
                    ?? ultimoRelatorio?.TradutorLibrasIdentificado
                    ?? false,
                HandTalkIdentificado = auditoria?.Summary.AssistiveTechnologies?.HandTalk ?? false
            };

            response.AchadosPorSeveridade = MapearSeveridades(auditoria);
            response.PontuacaoPorRota = MapearRotas(auditoria);
            response.CriteriosEmag = MapearCriteriosEmag(auditoria);

            return response;
        }

        private static RelatorioAuditoriaJson? DesserializarAuditoria(string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            try
            {
                return JsonSerializer.Deserialize<RelatorioAuditoriaJson>(json, JsonOptions);
            }
            catch (JsonException)
            {
                return null;
            }
        }

        private static List<DashboardSeveridadeResponse> MapearSeveridades(RelatorioAuditoriaJson? auditoria)
        {
            var porSeveridade = ObterQuantidadePorSeveridade(auditoria);

            return Enum.GetValues<SeveridadeEnum>()
                .Select(severidade => new DashboardSeveridadeResponse
                {
                    Severidade = severidade.ToDisplayName(),
                    Quantidade = porSeveridade.GetValueOrDefault(severidade)
                })
                .ToList();
        }

        private static Dictionary<SeveridadeEnum, int> ObterQuantidadePorSeveridade(RelatorioAuditoriaJson? auditoria)
        {
            if (auditoria?.Summary.BySeverity is { Count: > 0 } porSeveridade)
            {
                return porSeveridade;
            }

            if (auditoria?.Results is not { Count: > 0 })
            {
                return [];
            }

            return auditoria.Results
                .SelectMany(resultado => resultado.Findings)
                .GroupBy(finding => finding.Severity)
                .ToDictionary(grupo => grupo.Key, grupo => grupo.Count());
        }

        private static List<DashboardRotaResponse> MapearRotas(RelatorioAuditoriaJson? auditoria)
        {
            if (auditoria?.Results is not { Count: > 0 })
            {
                return [];
            }

            return auditoria.Results
                .Select(resultado => new DashboardRotaResponse
                {
                    Rota = string.IsNullOrWhiteSpace(resultado.Path) ? resultado.Url ?? "" : resultado.Path,
                    Pontuacao = resultado.Score,
                    ProblemasCriticos = resultado.CriticalIssues,
                    QuantidadeApontamentos = resultado.Findings.Count
                })
                .ToList();
        }

        private static List<DashboardEmagResponse> MapearCriteriosEmag(RelatorioAuditoriaJson? auditoria)
        {
            if (auditoria?.Results is not { Count: > 0 })
            {
                return [];
            }

            return auditoria.Results
                .SelectMany(resultado => resultado.Findings)
                .SelectMany(finding => finding.EmagCriteria)
                .Where(criterio => !string.IsNullOrWhiteSpace(criterio)
                    && !criterio.Contains("manual", StringComparison.OrdinalIgnoreCase))
                .GroupBy(criterio => criterio.Trim())
                .Select(grupo => new DashboardEmagResponse
                {
                    Criterio = grupo.Key,
                    Quantidade = grupo.Count()
                })
                .OrderByDescending(item => item.Quantidade)
                .ThenBy(item => item.Criterio)
                .Take(LimiteCriteriosEmag)
                .ToList();
        }
    }
}
