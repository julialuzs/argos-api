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
        /// Busca o projeto pelo Guid público informado
        /// </summary>
        /// <param name="guid">Identificador público do projeto</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Projeto</returns>
        [HttpGet("{guid:guid}")]
        [Authorize]
        public async Task<ActionResult<Projeto>> GetPorGuid(
            [FromRoute] Guid guid, CancellationToken cancellationToken = default)
        {
            var response = await projetosService.GetProjetoPorGuid(guid, cancellationToken);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        /// <summary>
        /// Busca os projetos pelo id do usuário informado
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Projetos</returns>
        [HttpGet("listar")]
        [Authorize]
        public async Task<ActionResult<List<Projeto>>> ListarProjetosPorUsuarioLogado(
            CancellationToken cancellationToken = default)
        {
            var response = await projetosService.ListarProjetosPorUsuarioLogado(cancellationToken);
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
        /// <param name="guid">Identificador público do projeto</param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        [HttpPut("{guid:guid}")]
        [Authorize]
        public async Task<ActionResult> Put(
            [FromRoute] Guid guid,
            [FromBody] CriacaoProjetoRequest request,
            CancellationToken cancellationToken = default)
        {
            var response = await projetosService.EditarProjeto(guid, request, cancellationToken);
            if (response == null)
            {
                return NotFound();
            }
            return Ok();
        }

        /// <summary>
        /// Vincular usuário no projeto 
        /// </summary>
        /// <param name="guid">Identificador público do projeto</param>
        /// <param name="idUsuario"></param>
        /// <param name="cancellationToken"></param>
        [HttpPut("{guid:guid}/vincular-usuario/{idUsuario}")]
        [Authorize]
        public async Task<ActionResult> VincularUsuarioNoProjeto(
            [FromRoute] Guid guid,
            [FromRoute] int idUsuario, 
            CancellationToken cancellationToken = default)
        { 
            var response = await projetosService.VincularUsuarioNoProjeto(guid, idUsuario, cancellationToken);
            if (response == null)
            {
                return NotFound();
            }
            return Ok();
        }

        /// <summary>
        /// Remove o projeto pelo Guid informado
        /// </summary>
        /// <param name="guid">Identificador público do projeto</param>
        [HttpDelete("{guid:guid}")]
        [Authorize]
        public ActionResult<string> Delete(Guid guid)
        {
            return "";
        }
    }
}
