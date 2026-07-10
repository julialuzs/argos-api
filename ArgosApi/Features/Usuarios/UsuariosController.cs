using ArgosApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ArgosApi.Features.Usuarios
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuariosService _usuariosService;
        public UsuariosController(
            UsuariosService usuariosService
        )
        {
            this._usuariosService = usuariosService;
        }

        /// <summary>
        /// Busca o usuário pelo id informado
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Usuario</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetPorId([FromRoute] long id, CancellationToken cancellationToken = default)
        {
            var response = await _usuariosService.GetUsuarioPorId(id, cancellationToken);
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
        public ActionResult<string> Post([FromBody] Usuario usuario, CancellationToken cancellationToken = default)
        {
            _usuariosService.CriarUsuario(usuario, cancellationToken);
            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult<string> Put(int id, [FromBody] string value)
        {
            return "";
        }

        [HttpDelete("{id}")]
        public ActionResult<string> Delete(int id)
        {
            return "";
        }
    }
}