using ArgosApi.Data;
using ArgosApi.Infrastructure.Authentication;
using Microsoft.EntityFrameworkCore;

namespace ArgosApi.Features.Authentication
{
    public class AuthService(AppDbContext context, JwtService jwtService)
    {
        public async Task<LoginResponse> LoginAsync(
            LoginRequest request,
            CancellationToken cancellationToken = default)
        {
            var usuario = await context.Usuarios
                .FirstOrDefaultAsync(
                    x => x.Email == request.Email,
                    cancellationToken);

            if (usuario is null || !BCrypt.Net.BCrypt.Verify(request.Senha, usuario.SenhaHash))
                throw new AuthenticationException("Usuário ou senha inválidos.");

            var senhaValida = BCrypt.Net.BCrypt.Verify(
                request.Senha,
                usuario.SenhaHash);

            if (!senhaValida)
                throw new AuthenticationException("Usuário ou senha inválidos.");

            var token = jwtService.GenerateToken(usuario);

            return new LoginResponse
            {
                Token = token.Token,
                Expiration = token.Expiration
            };
        }
    }
}
