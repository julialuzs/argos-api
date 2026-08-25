namespace ArgosApi.Infrastructure.Authentication
{
    /// <summary>
    /// Token JWT gerado para o usuário autenticado
    /// </summary>
    public class JwtToken
    {
        /// <summary>
        /// Valor do token
        /// </summary>
        public string Token { get; init; } = string.Empty;

        /// <summary>
        /// Data/hora de expiração do token
        /// </summary>
        public DateTime Expiration { get; init; }
    }
}
