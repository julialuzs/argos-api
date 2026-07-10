using ArgosApi.Data;
using ArgosApi.Domain.Entities;

namespace ArgosApi.Features.Usuarios
{
    public class UsuariosService
    {
        private readonly AppDbContext _context;

        public UsuariosService(AppDbContext context)
        {
            this._context = context;
        }

        public async void CriarUsuario(Usuario usuario, CancellationToken cancellationToken)
        {
            await _context.Usuarios.AddAsync(usuario, cancellationToken);
            _context.SaveChanges();
        }

        public async Task<Usuario?> GetUsuarioPorId(long id, CancellationToken cancellationToken)
        {
            return await _context.Usuarios.FindAsync(id, cancellationToken);
        }

    }
}