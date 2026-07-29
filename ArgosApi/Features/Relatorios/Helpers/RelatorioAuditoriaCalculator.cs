namespace ArgosApi.Features.Relatorios.Helpers
{
    /// <summary>
    /// Calculador responsável por calcular a pontuação do relatorio
    /// </summary>
    public class RelatorioAuditoriaCalculator( )
    {
        public static int CalcularPontuacao(RelatorioAuditoriaJson? auditoria)
        {
            if (auditoria?.Results is not { Count: > 0 } results)
            {
                return 0;
            }

            return (int)Math.Round(results.Average(resultado => resultado.Score));
        }

        public static int ContarApontamentosPorSeveridade(
            RelatorioAuditoriaJson? auditoria,
            params string[] severidades)
        {
            if (auditoria?.Results is not { Count: > 0 })
            {
                return 0;
            }

            return auditoria.Results
                .SelectMany(resultado => resultado.Findings)
                .Count(finding =>
                {
                    var nivel = finding.Severity ?? finding.Impact ?? string.Empty;
                    return severidades.Contains(nivel, StringComparer.OrdinalIgnoreCase);
                });
        }
    }
}