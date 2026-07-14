using ArgosApi.Infrastructure.Authentication; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArgosApi.Features.Authentication
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController(
        AuthService authService
    ) : ControllerBase
    {
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
