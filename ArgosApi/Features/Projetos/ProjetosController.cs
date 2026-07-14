using ArgosApi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArgosApi.Features.Projetos
{
    /// <summary>
    /// Controller responsável por gerenciar os projetos
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ProjetosController(
        ProjetosService projetosService
    ) : ControllerBase
    {
        /// <summary>
        /// Busca o projeto pelo id informado
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Projeto</returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Projeto>> GetPorId(
            [FromRoute] long id, CancellationToken cancellationToken = default)
        {
            var response = await projetosService.GetProjetoPorId(id, cancellationToken);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        /// <summary>
        /// Busca os projetos pelo id do usuário informado
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Projetos</returns>
        [HttpGet("listar/{id}")]
        [Authorize]
        public async Task<ActionResult<List<Projeto>>> ListarProjetosPorIdUsuario(
            [FromRoute] long id, CancellationToken cancellationToken = default)
        {
            var response = await projetosService.ListarProjetosPorUsuario(id, cancellationToken);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        /// <summary>
        /// Cria o projeto 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CriarProjeto([FromBody] CriacaoProjetoRequest request, CancellationToken cancellationToken = default)
        {
            await projetosService.CriarProjeto(request, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Altera informações do projeto 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="projeto"></param>
        /// <param name="cancellationToken"></param>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> Put(
            [FromRoute] int id,
            [FromBody] Projeto projeto,
            CancellationToken cancellationToken = default)
        {
            projeto.Id = id;
            var response = await projetosService.EditarProjeto(projeto, cancellationToken);
            if (response == null)
            {
                return NotFound();
            }
            return Ok();
        }

        /// <summary>
        /// Vincular usuário no projeto 
        /// </summary>
        /// <param name="id">id do projeto</param>
        /// <param name="idUsuario"></param>
        /// <param name="cancellationToken"></param>
        [HttpPut("{id}/vincular-usuario/{idUsuario}")]
        [Authorize]
        public async Task<ActionResult> VincularUsuarioNoProjeto(
            [FromRoute] int id,
            [FromRoute] int idUsuario, 
            CancellationToken cancellationToken = default)
        { 
            var response = await projetosService.VincularUsuarioNoProjeto(id, idUsuario, cancellationToken);
            if (response == null)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult<string> Delete(int id)
        {
            return "";
        }
    }
}