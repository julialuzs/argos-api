using System.Text.Json;
using ArgosApi.Domain.Entities;

namespace ArgosApi.Features.Relatorios.Helpers
{
    /// <summary>
    /// Mapeador responsável por mapear a auditoria do relatorio
    /// </summary>
    public class RelatorioAuditoriaMapper( )
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
        public static RelatorioDetalheResponse MapearParaDetalhe(Relatorio relatorio)
        {
            var auditoria = JsonSerializer.Deserialize<RelatorioAuditoriaJson>(relatorio.Json, JsonOptions);

            return new RelatorioDetalheResponse
            {
                Id = relatorio.Id,
                ProjetoId = relatorio.ProjetoId,
                DataHoraExecucao = relatorio.DataHoraExecucao,
                Pontuacao = relatorio.Pontuacao,
                TradutorLibrasIdentificado = relatorio.TradutorLibrasIdentificado,
                HandTalkIdentificado = auditoria?.AssistiveTechnologies?.HandTalk ?? false,
                QuantidadeErros = relatorio.QuantidadeErros,
                QuantidadeAvisos = relatorio.QuantidadeAvisos,
                RotasAuditadas = auditoria?.RoutesAudited ?? auditoria?.Results.Count ?? 0,
                FluxosAuditados = auditoria?.FlowsAudited ?? 0,
                Resultados = MapearResultados(auditoria)
            };
        }

        public static List<ResultadoAuditoriaResponse> MapearResultados(RelatorioAuditoriaJson? auditoria)
        {
            if (auditoria?.Results is not { Count: > 0 } resultados)
            {
                return [];
            }

            return resultados.Select(resultado => new ResultadoAuditoriaResponse
            {
                Url = resultado.Url ?? string.Empty,
                Caminho = resultado.Path ?? string.Empty,
                Pontuacao = resultado.Score,
                ProblemasCriticos = resultado.CriticalIssues,
                CriteriosEmagMapeados = resultado.EmagMappings,
                Apontamentos = resultado.Findings.Select(MapearApontamento).ToList()
            }).ToList();
        }
       
        public static ApontamentoResponse MapearApontamento(ApontamentoJson apontamento)
        {
            var severidade = apontamento.Severity ?? apontamento.Impact ?? string.Empty;

            return new ApontamentoResponse
            {
                Id = apontamento.Id ?? string.Empty,
                Titulo = apontamento.Title ?? string.Empty,
                Severidade = severidade,
                Tipo = ObterTipoApontamento(severidade),
                Fonte = apontamento.Source ?? string.Empty,
                Descricao = apontamento.Description ?? string.Empty,
                CriteriosEmag = apontamento.EmagCriteria,
                Recomendacao = apontamento.Recommendation ?? string.Empty,
                UrlAjuda = apontamento.HelpUrl,
                ReferenciasWcag = apontamento.WcagRefs,
                ElementoHtml = apontamento.HtmlElement,
                QuantidadeElementos = apontamento.ElementCount
            };
        }

        public static string ObterTipoApontamento(string severidade) =>
            severidade is "serious" or "critical" ? "erro" : "aviso";

    }
}