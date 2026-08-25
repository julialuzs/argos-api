namespace ArgosApi.Features.Authentication
{
    /// <summary>
    /// Exceção lançada quando a autenticação falha
    /// </summary>
    public class AuthenticationException : Exception
    {
        /// <summary>
        /// Inicializa a exceção com a mensagem informada
        /// </summary>
        public AuthenticationException(string message)
            : base(message)
        {
        }
    }
}