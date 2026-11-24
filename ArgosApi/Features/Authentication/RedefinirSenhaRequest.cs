using System.ComponentModel.DataAnnotations;

namespace ArgosApi.Features.Authentication
{
    /// <summary>
    /// Dados para redefinir a senha de um e-mail já cadastrado
    /// </summary>
    public class RedefinirSenhaRequest
    {
        /// <summary>
        /// E-mail do usuário
        /// </summary>
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
        public required string Email { get; set; }

        /// <summary>
        /// Nova senha do usuário
        /// </summary>
        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(
            100,
            MinimumLength = 8,
            ErrorMessage = "A senha deve ter no mínimo 8 caracteres.")]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
            ErrorMessage = "A senha deve conter letras maiúsculas, minúsculas e números.")]
        public required string Senha { get; set; }

        /// <summary>
        /// Confirmação da nova senha
        /// </summary>
        [Required(ErrorMessage = "A confirmação de senha é obrigatória.")]
        [Compare(
            nameof(Senha),
            ErrorMessage = "A confirmação de senha não corresponde à senha.")]
        public required string ConfirmacaoSenha { get; set; }
    }
}
