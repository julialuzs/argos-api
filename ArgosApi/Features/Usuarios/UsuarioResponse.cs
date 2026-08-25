using ArgosApi.Domain.Entities;

namespace ArgosApi.Features.Usuarios
{
    /// <summary>
    /// Dados do usuário retornados pela API
    /// </summary>
    public class UsuarioResponse
    {
        /// <summary>
        /// Nome do usuário
        /// </summary>
        public required string Nome { get; set; }

        /// <summary>
        /// E-mail do usuário
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Projeto atualmente selecionado
        /// </summary>
        public Projeto? ProjetoSelecionado { get; set; }

        /// <summary>
        /// Projetos vinculados ao usuário
        /// </summary>
        public IEnumerable<Projeto> Projetos { get; set; } = [];
    }
}