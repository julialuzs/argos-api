using ArgosApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ArgosApi.Features.Usuarios
{
    /// <summary>
    /// Controller responsável por gerenciar os usuários
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController(
        UsuariosService usuariosService
    ) : ControllerBase
    {
        /// <summary>
        /// Busca o usuário pelo id informado
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Usuario</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetPorId(
            [FromRoute] long id, CancellationToken cancellationToken = default)
        {
            var response = await usuariosService.GetUsuarioPorId(id, cancellationToken);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        /// <summary>
        /// Cria o usuário 
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Usuario</returns>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Usuario usuario, CancellationToken cancellationToken = default)
        {
            await usuariosService.CriarUsuario(usuario, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Altera informações do usuário 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(
            [FromRoute] int id, 
            [FromBody] Usuario usuario, 
            CancellationToken cancellationToken = default)
        {
            usuario.Id = id;
            var response = await usuariosService.EditarUsuario(usuario, cancellationToken); 
            if (response == null)
            {
                return NotFound();
            } 
            return Ok();
        }

        /// <summary>
        /// Deleta o usuário 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            // implementar soft delete
            return Ok();
        }
    }
}