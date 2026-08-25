using System.Security.Claims;

namespace ArgosApi.Features.Usuarios
{
    /// <summary>
    /// Dados do usuário autenticado extraídos do token
    /// </summary>
    public class CurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Inicializa o usuário atual a partir do contexto HTTP
        /// </summary>
        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal User =>
            _httpContextAccessor.HttpContext?.User
            ?? throw new InvalidOperationException("Usuário não autenticado.");

        /// <summary>
        /// Identificador do usuário autenticado
        /// </summary>
        public long Id =>
            long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        /// <summary>
        /// Nome do usuário autenticado
        /// </summary>
        public string Nome =>
            User.FindFirstValue(ClaimTypes.Name)!;

        /// <summary>
        /// E-mail do usuário autenticado
        /// </summary>
        public string Email =>
            User.FindFirstValue(ClaimTypes.Email)!;

    }
}
