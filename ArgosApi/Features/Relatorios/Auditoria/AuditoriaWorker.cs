namespace ArgosApi.Features.Relatorios.Auditoria
{
    public class AuditoriaWorker(
        AuditoriaQueue queue,
        AuditoriaExecutor executor,
        ILogger<AuditoriaWorker> logger) : BackgroundService
    {
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
