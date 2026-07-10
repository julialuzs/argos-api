using ArgosApi.Data;
using ArgosApi.Domain.Entities;

namespace ArgosApi.Features.Relatorios
{
    /// <summary>
    /// Service responsável por gerenciar os relatórios
    /// </summary>
    public class RelatoriosService(AppDbContext context)
    {
        /// <summary>
        /// Busca relatório pelo id
        /// </summary>
        public async Task<Relatorio?> GetRelatorioPorId(long id, CancellationToken cancellationToken)
        {
            return await context.Relatorios.FindAsync(id, cancellationToken);
        }

        /// <summary>
        /// Busca todos os relatorios pelo id do projeto
        /// </summary>
        public async Task<List<Relatorio>> ListarRelatoriosPorProjeto(long idProjeto, CancellationToken cancellationToken)
        {
            return context.Relatorios.Where((relatorio) =>
                relatorio.ProjetoId == idProjeto).ToList() ?? [];
        }

        /// <summary>
        /// Salvar relatorio na base de dados
        /// </summary>
        public async Task SalvarRelatorio(RelatorioRequest request, CancellationToken cancellationToken)
        {
            //destrinchar json depois
            var relatorio = new Relatorio
            {
                Json = request.Json.GetRawText(),
                ProjetoId = request.IdProjeto, 
                DataHoraExecucao = DateTime.UtcNow
            };
            await context.Relatorios.AddAsync(relatorio, cancellationToken);
            await context.SaveChangesAsync();
        }
    }
}