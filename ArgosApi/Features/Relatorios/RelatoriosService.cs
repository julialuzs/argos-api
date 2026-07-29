using System.Text.Json;
using ArgosApi.Data;
using ArgosApi.Domain.Entities;
using ArgosApi.Features.Relatorios.Helpers;

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
            var auditoria = JsonSerializer.Deserialize<RelatorioAuditoriaJson>(jsonText, JsonOptions);

            var relatorio = new Relatorio
            {
                Json = jsonText,
                ProjetoId = request.IdProjeto,
                DataHoraExecucao = auditoria?.AuditDate ?? DateTime.UtcNow,
                Pontuacao = RelatorioAuditoriaCalculator.CalcularPontuacao(auditoria),
                TradutorLibrasIdentificado = auditoria?.AssistiveTechnologies?.VLibras ?? false,
                QuantidadeErros = RelatorioAuditoriaCalculator.ContarApontamentosPorSeveridade(auditoria, "serious", "critical"),
                QuantidadeAvisos = RelatorioAuditoriaCalculator.ContarApontamentosPorSeveridade(auditoria, "moderate", "minor")
            };

            await context.Relatorios.AddAsync(relatorio, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

    }
}