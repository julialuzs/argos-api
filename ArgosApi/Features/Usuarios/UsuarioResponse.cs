using ArgosApi.Domain.Entities;

namespace ArgosApi.Features.Usuarios
{
    public class UsuarioResponse
    {
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public Projeto? ProjetoSelecionado { get; set; }
        public IEnumerable<Projeto> Projetos { get; set; } = [];
    }
}