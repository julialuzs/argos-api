using ArgosApi.Data;
using ArgosApi.Infrastructure.Authentication;
using Microsoft.EntityFrameworkCore;

namespace ArgosApi.Features.Authentication
{
    /// <summary>
    /// Service responsável pela autenticação de usuários
    /// </summary>
    public class AuthService(AppDbContext context, JwtService jwtService)
    {
        /// <summary>
        /// Valida as credenciais e gera o token JWT
        /// </summary>
        public async Task<LoginResponse> LoginAsync(
            LoginRequest request,
            CancellationToken cancellationToken = default)
        {
            var usuario = await context.Usuarios
                .FirstOrDefaultAsync(
                    x => x.Email == request.Email,
                    cancellationToken);

            if (usuario is null)
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

        /// <summary>
        /// Redefine a senha de quem informar um e-mail já cadastrado.
        /// Não há confirmação por e-mail: quem souber o e-mail consegue trocar a senha.
        /// </summary>
        /// <returns>false quando o e-mail não está cadastrado</returns>
        public async Task<bool> RedefinirSenhaAsync(
            RedefinirSenhaRequest request,
            CancellationToken cancellationToken = default)
        {
            var usuario = await context.Usuarios
                .FirstOrDefaultAsync(
                    x => x.Email == request.Email,
                    cancellationToken);

            if (usuario is null)
                return false;

            usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha);
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
