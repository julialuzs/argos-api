namespace ArgosApi.Features.Projetos
{
    /// <summary>
    /// Request para criação de projeto
    /// </summary>
    public class CriacaoProjetoRequest
    {
        /// <summary>
        /// Nome do projeto
        /// </summary>
        public string Nome { get; set; } = string.Empty;
        /// <summary>
        /// Descrição do projeto
        /// </summary>
        public string Descricao { get; set; } = string.Empty;
        /// <summary>
        /// Usuario criador do projeto
        /// </summary>
        public long IdUsuario { get; set; }
    }
}