using ArgosApi.Data;
using ArgosApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArgosApi.Features.Usuarios
{
    /// <summary>
    /// Service responsável por gerenciar os usuários
    /// </summary>
    public class UsuariosService(AppDbContext context)
    {
        /// <summary>
        /// Cria usuário na base de dados
        /// </summary>
        public async Task CriarUsuario(CriacaoUsuarioRequest request, CancellationToken cancellationToken)
        {
            var usuarioExistenteNoBanco = await context.Usuarios.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
            
            if (usuarioExistenteNoBanco != null)
            {
                throw new Exception("E-mail já cadastrado na aplicação");
            }
            var hash = BCrypt.Net.BCrypt.HashPassword(request.Senha);

            Usuario usuario = new()
            {
                Email = request.Email,
                Nome = request.Nome,
                SenhaHash = hash
            };

            await context.Usuarios.AddAsync(usuario, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Busca usuário pelo id e altera os dados na base de dados
        /// </summary>
        public async Task<Usuario?> AlterarUsuario(Usuario usuario, CancellationToken cancellationToken)
        {
            var usuarioDb = await context.Usuarios.FindAsync([usuario.Id], cancellationToken: cancellationToken); 
            if (usuarioDb == null)
            {
                return null;
            }
            usuarioDb = usuario;
            await context.SaveChangesAsync(cancellationToken);
            return usuarioDb;
        }

        /// <summary>
        /// Busca usuário pelo id
        /// </summary>
        public async Task<Usuario?> GetUsuarioPorId(long id, CancellationToken cancellationToken)
        {
            return await context.Usuarios.FindAsync([id], cancellationToken: cancellationToken);
        }

    }
}