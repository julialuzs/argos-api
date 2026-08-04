using System.Text.Json;
using ArgosApi.Data;
using ArgosApi.Domain.Entities;
using ArgosApi.Features.Relatorios.Helpers;
using ArgosApi.Features.Relatorios.Requests;
using ArgosApi.Features.Relatorios.Responses;

namespace ArgosApi.Features.Relatorios
{
    /// <summary>
    /// Service responsável por gerenciar os relatórios
    /// </summary>
    public class RelatoriosService(AppDbContext context)
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
                .OrderBy(r => r.DataHoraExecucao);
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
                Pontuacao = RelatorioAuditoriaCalculator.CalcularPontuacao(auditoria),
                TradutorLibrasIdentificado = auditoria?.Summary.AssistiveTechnologies?.VLibras ?? false,
                QuantidadeErros = RelatorioAuditoriaCalculator.ContarApontamentosPorSeveridade(auditoria, SeveridadeEnum.Serious, SeveridadeEnum.Critical),
                QuantidadeAvisos = RelatorioAuditoriaCalculator.ContarApontamentosPorSeveridade(auditoria, SeveridadeEnum.Moderate, SeveridadeEnum.Minor)
            };

            await context.Relatorios.AddAsync(relatorio, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

    }
}