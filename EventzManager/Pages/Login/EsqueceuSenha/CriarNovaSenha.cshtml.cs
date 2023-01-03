using EventzManager.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EventzManager.Pages.Login.EsqueceuSenha
{
    public class CriarNovaSenhaModel : PageModel
    {
        [DisplayName("Senha")]
        [Required(ErrorMessage = "O campo de '{0}' é requerido.")]
        [MaxLength(60, ErrorMessage = "O campo de '{0}' só pode conter no máximo 60 caracteres.")]
        public string NovaSenha { get; set; } = string.Empty;
        
        [DisplayName("Confirmar senha")]
        [Required(ErrorMessage = "O campo de '{0}' é requerido.")]
        [MaxLength(60, ErrorMessage = "O campo de '{0}' só pode conter no máximo 60 caracteres.")]
        public string ConfirmacaoSenha { get; set; } = string.Empty;

        private readonly BancoDeDados Contexto;

        public CriarNovaSenhaModel(BancoDeDados contexto)
        {
            Contexto = contexto;
        }

        public void OnGet(uint id)
        {
            Usuario? usuario = Contexto.Usuarios.Find(id);

            if (usuario != null) //checa para caso a conta exista
                Response.Cookies.Append("id_usuario", id.ToString());
        }

        public IActionResult OnPostSalvar()
        {
            string? cookieId = Request.Cookies["id_usuario"];

            return RedirectToPage("/Principal/ListaEventos", new { id = cookieId });
        }
    }
}
