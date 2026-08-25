namespace ArgosApi.Infrastructure.Authentication
{
    /// <summary>
    /// Credenciais enviadas no login
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// E-mail do usuário
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Senha do usuário
        /// </summary>
        public string Senha { get; set; } = string.Empty;
    }
}
