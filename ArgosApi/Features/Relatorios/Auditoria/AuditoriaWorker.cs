namespace ArgosApi.Features.Relatorios.Auditoria
{
    /// <summary>
    /// Worker em background que consome a fila de auditorias
    /// </summary>
    public class AuditoriaWorker(
        AuditoriaQueue queue,
        AuditoriaExecutor executor,
        ILogger<AuditoriaWorker> logger) : BackgroundService
    {
        /// <summary>
        /// Consome a fila e dispara a execução das auditorias
        /// </summary>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var projetoId in queue.ReadAllAsync(stoppingToken))
            {
                try
                {
                    await executor.ExecutarAsync(projetoId, stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Falha ao executar auditoria do projeto {ProjetoId}", projetoId);
                }
            }
        }
    }
}
