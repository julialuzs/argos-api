namespace ArgosApi.Features.Projetos
{
    /// <summary>
    /// Request para criação ou edição de projeto
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
        public string? Descricao { get; set; }

        /// <summary>
        /// URL base do site a ser auditado
        /// </summary>
        public string UrlBase { get; set; } = string.Empty;

        /// <summary>
        /// Rotas públicas a auditar
        /// </summary>
        public string[] Rotas { get; set; } = ["/"];

        /// <summary>
        /// Indica se a validação W3C deve ser incluída
        /// </summary>
        public bool IncluirW3c { get; set; }
    }
}
