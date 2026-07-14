namespace ArgosApi.Features.Usuarios
{
    /// <summary>
    /// Request para criação de usuário
    /// </summary>
    public class CriacaoUsuarioRequest
    {
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string Senha { get; set; }
    }
}
