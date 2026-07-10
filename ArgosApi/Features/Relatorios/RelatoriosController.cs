using ArgosApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ArgosApi.Features.Relatorios
{
    /// <summary>
    /// Controller responsável por gerenciar os projetos
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RelatoriosController(
        RelatoriosService relatoriosService
    ) : ControllerBase
    {
        /// <summary>
        /// Busca o relatorio pelo id informado
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Projeto</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Projeto>> GetPorId(
            [FromRoute] long id, CancellationToken cancellationToken = default)
        {
            var response = await relatoriosService.GetRelatorioPorId(id, cancellationToken);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        /// <summary>
        /// Busca os relatorios pelo id do projeto informado
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Projetos</returns>
        [HttpGet("listar/{id}")]
        public async Task<ActionResult<List<Relatorio>>> ListarRelatoriosPorProjeto(
            [FromRoute] long id, CancellationToken cancellationToken = default)
        {
            var response = await relatoriosService.ListarRelatoriosPorProjeto(id, cancellationToken);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        /// <summary>
        /// Salva o relatorio em JSON
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> SalvarRelatorio([FromBody] RelatorioRequest request, CancellationToken cancellationToken = default)
        {
            await relatoriosService.SalvarRelatorio(request, cancellationToken);
            return Ok();
        }
    }
}