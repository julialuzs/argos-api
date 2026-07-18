using System.ComponentModel.DataAnnotations;

namespace ArgosApi.Features.Usuarios
{
    /// <summary>
    /// Request para criação de usuário
    /// </summary>
    public class CriacaoUsuarioRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(
        80,
        MinimumLength = 2,
        ErrorMessage = "O nome deve ter entre 2 e 80 caracteres.")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(
        100,
        MinimumLength = 8,
        ErrorMessage = "A senha deve ter no mínimo 8 caracteres.")]
        [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
        ErrorMessage = "A senha deve conter letras maiúsculas, minúsculas e números.")]
        public required string Senha { get; set; }

        [Required(ErrorMessage = "A confirmação de senha é obrigatória.")]
        [Compare(
        nameof(Senha),
        ErrorMessage = "A confirmação de senha não corresponde à senha.")]
        public required string ConfirmacaoSenha { get; set; }
    }
}
