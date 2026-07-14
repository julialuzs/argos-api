namespace ArgosApi.Features.Authentication
{
    public class AuthenticationException : Exception
    {
        public AuthenticationException(string message)
            : base(message)
        {
        }
    }
}