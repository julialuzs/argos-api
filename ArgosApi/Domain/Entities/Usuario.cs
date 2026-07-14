namespace ArgosApi.Domain.Entities
{
    /// <summary>
    /// Entidade de usuário
    /// </summary>
    public class Usuario : BaseEntity
    {
        /// <summary>
        /// Nome do usuário
        /// </summary>
        public string Nome { get; set; } = string.Empty;
        /// <summary>
        /// Email do usuário
        /// </summary>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// Senha em hash do usuário
        /// </summary>
        public string SenhaHash { get; set; } = string.Empty;
        /// <summary>
        /// Projetos vinculados ao usuário
        /// </summary>
        public ICollection<Projeto> Projetos { get; set; } = [];
    }
}