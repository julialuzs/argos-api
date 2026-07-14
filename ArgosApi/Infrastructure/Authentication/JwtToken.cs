namespace ArgosApi.Infrastructure.Authentication
{
    public class JwtToken
    {
        public string Token { get; init; } = string.Empty;

        public DateTime Expiration { get; init; }
    }
}
