using System.Security.Claims;

namespace ArgosApi.Features.Usuarios
{
    public class CurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal User =>
            _httpContextAccessor.HttpContext?.User
            ?? throw new InvalidOperationException("Usuário não autenticado.");

        public long Id =>
            long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public string Nome =>
            User.FindFirstValue(ClaimTypes.Name)!;

        public string Email =>
            User.FindFirstValue(ClaimTypes.Email)!;

    }
}
