using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EventzManager.ViewsModelos
{
    public class UsuarioView
    {
        [DisplayName("Nome")]
        [Required(ErrorMessage = "O campo de '{0}' é requerido.")]
        [MaxLength(80, ErrorMessage = "O campo de '{0}' só pode conter no máximo 80 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [DisplayName("Email")]
        [Required(ErrorMessage = "O campo de '{0}' é requerido.")]
        [EmailAddress(ErrorMessage = "Por favor, insira um email válido.")]
        [MaxLength(320, ErrorMessage = "O campo de '{0}' só pode conter no máximo 320 caracteres.")]
        public string Email { get; set; } = string.Empty;

        [DisplayName("Senha")]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=])(?=\S+$).{8,}$", ErrorMessage = $"A senha precisa conter:" +
            $"<br/ >- No mínimo 8 caracteres." +
            $"<br/ >- Uma letra em caixa alta [A-Z]." +
            $"<br/ >- Uma letra em caixa baixa [a-z]." +
            $"<br/ >- Um número [0-9]." +
            $"<br/ >- Um caractere especial [@#$%^&+=].")]
        [Required(ErrorMessage = "O campo de '{0}' é requerido.")]
        [MaxLength(60, ErrorMessage = "O campo de '{0}' só pode conter no máximo 60 caracteres.")]
        public string Senha { get; set; } = string.Empty;
    }
}
