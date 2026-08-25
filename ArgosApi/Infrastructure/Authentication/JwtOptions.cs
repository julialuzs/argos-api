namespace ArgosApi.Infrastructure.Authentication
{
    /// <summary>
    /// Opções de configuração do JWT
    /// </summary>
    public class JwtOptions
    {
        /// <summary>
        /// Nome da seção no arquivo de configuração
        /// </summary>
        public const string SectionName = "Jwt";

        /// <summary>
        /// Chave de assinatura do token
        /// </summary>
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// Emissor do token
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// Audiência do token
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// Tempo de expiração do token, em minutos
        /// </summary>
        public int ExpirationMinutes { get; set; }
    }
}
