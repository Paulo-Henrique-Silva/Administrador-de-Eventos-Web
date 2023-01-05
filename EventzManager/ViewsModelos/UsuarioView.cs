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
        [Required(ErrorMessage = "O campo de '{0}' é requerido.")]
        [MaxLength(60, ErrorMessage = "O campo de '{0}' só pode conter no máximo 60 caracteres.")]
        public string Senha { get; set; } = string.Empty;
    }
}
