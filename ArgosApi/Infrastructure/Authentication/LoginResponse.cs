namespace ArgosApi.Infrastructure.Authentication
{
    /// <summary>
    /// Resposta do login com o token JWT
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// Token de autenticação
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Data/hora de expiração do token
        /// </summary>
        public DateTime Expiration { get; set; }
    }
}
