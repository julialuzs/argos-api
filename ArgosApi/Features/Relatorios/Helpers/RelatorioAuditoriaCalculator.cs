using ArgosApi.Features.Relatorios.Requests;

namespace ArgosApi.Features.Relatorios.Helpers
{
    /// <summary>
    /// Calculador responsável por calcular a pontuação do relatorio
    /// </summary>
    public class RelatorioAuditoriaCalculator( )
    {

        /// <summary>
        /// Conta apontamentos da auditoria filtrando pelas severidades informadas
        /// </summary>
        public static int ContarApontamentosPorSeveridade(
            RelatorioAuditoriaJson? auditoria,
            params SeveridadeEnum[] severidades)
        {
            if (auditoria?.Results is not { Count: > 0 })
            {
                return 0;
            }

            return auditoria.Results
                .SelectMany(resultado => resultado.Findings)
                .Count(finding =>
                { 
                    return severidades.Contains(finding.Severity);
                });
        }
    }
}