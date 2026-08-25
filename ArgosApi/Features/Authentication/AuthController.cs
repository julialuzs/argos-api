using ArgosApi.Infrastructure.Authentication; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArgosApi.Features.Authentication
{
    /// <summary>
    /// Controller responsável pela autenticação
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AuthController(
        AuthService authService
    ) : ControllerBase
    {
        /// <summary>
        /// Autentica o usuário e retorna o token JWT
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Login(
            LoginRequest request,
            CancellationToken cancellationToken)
        {
            var response = await authService.LoginAsync(
                request,
                cancellationToken);

            return Ok(response);
        }
    }
}
