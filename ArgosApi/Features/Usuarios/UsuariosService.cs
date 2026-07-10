using ArgosApi.Data;
using ArgosApi.Domain.Entities;

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
        public async Task CriarUsuario(Usuario usuario, CancellationToken cancellationToken)
        {
            await context.Usuarios.AddAsync(usuario, cancellationToken);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Busca usuário pelo id e altera os dados na base de dados
        /// </summary>
        public async Task<Usuario?> EditarUsuario(Usuario usuario, CancellationToken cancellationToken)
        {
            var usuarioDb = await context.Usuarios.FindAsync(usuario.Id, cancellationToken); 
            if (usuarioDb == null)
            {
                return null;
            }
            usuarioDb = usuario;
            await context.SaveChangesAsync();
            return usuarioDb;
        }

        /// <summary>
        /// Busca usuário pelo id
        /// </summary>
        public async Task<Usuario?> GetUsuarioPorId(long id, CancellationToken cancellationToken)
        {
            return await context.Usuarios.FindAsync(id, cancellationToken);
        }

    }
}