using System.Text.Json;
using ArgosApi.Domain.Entities;
using ArgosApi.Features.Relatorios.Requests;
using ArgosApi.Features.Relatorios.Responses;

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
        /// <summary>
        /// Mapeia a entidade de relatório para o detalhe exibido na API
        /// </summary>
        public static RelatorioDetalheResponse MapearParaDetalhe(Relatorio relatorio)
        {
            var auditoria = JsonSerializer.Deserialize<RelatorioAuditoriaJson>(relatorio.Json, JsonOptions);

            return new RelatorioDetalheResponse
            {
                Id = relatorio.Id,
                ProjetoId = relatorio.ProjetoId,
                DataHoraExecucao = relatorio.DataHoraExecucao,
                Pontuacao = relatorio.Pontuacao,
                VLibrasIdentificado = auditoria?.Summary.AssistiveTechnologies?.VLibras ?? false,
                HandTalkIdentificado = auditoria?.Summary.AssistiveTechnologies?.HandTalk ?? false,
                QuantidadeErros = relatorio.QuantidadeErros,
                QuantidadeAvisos = relatorio.QuantidadeAvisos,
                RotasAuditadas = auditoria?.Summary.RoutesAudited ?? auditoria?.Results.Count ?? 0,
                FluxosAuditados = auditoria?.Summary.FlowsAudited ?? 0,
                Resultados = MapearResultados(auditoria)
            };
        }

        /// <summary>
        /// Mapeia os resultados por rota da auditoria
        /// </summary>
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
                Apontamentos = [
                    .. resultado.Findings.OrderBy(a => a.Severity)
                    .ThenBy(a => a.Title)
                    .Select(MapearApontamento)
                ]
            }).ToList();
        }
       
        /// <summary>
        /// Mapeia um apontamento do JSON do avaliador para a resposta da API
        /// </summary>
        public static ApontamentoResponse MapearApontamento(ApontamentoJson apontamento)
        { 
            return new ApontamentoResponse
            {
                Id = apontamento.Id ?? string.Empty,
                Titulo = apontamento.Title ?? string.Empty,
                Severidade = apontamento.Severity.ToDisplayName(),
                Tipo = ObterTipoApontamento(apontamento.Severity),
                Fonte = apontamento.Source ?? string.Empty,
                Descricao = apontamento.Description ?? string.Empty,
                CriteriosEmag = apontamento.EmagCriteria,
                Recomendacao = apontamento.Recommendation ?? string.Empty,
                UrlAjuda = apontamento.HelpUrl,
                ReferenciasWcag = apontamento.WcagRefs,
                ElementoHtml = apontamento.HtmlElement,
                SeletorCss = apontamento.CssSelector,
                QuantidadeElementos = apontamento.ElementCount
            };
        }

        /// <summary>
        /// Obtém o tipo do apontamento a partir da severidade
        /// </summary>
        public static string ObterTipoApontamento(SeveridadeEnum severidade) =>
            severidade is SeveridadeEnum.Serious or SeveridadeEnum.Critical ? "erro" : "aviso";

    }
}