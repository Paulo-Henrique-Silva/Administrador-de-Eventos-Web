using EventzManager.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventzManager.Pages.Login.EsqueceuSenha
{
    public class BuscarEmailModel : PageModel
    {
        [BindProperty]
        [DisplayName("Email")]
        [Required(ErrorMessage = "O campo de '{0}' � requerido.")]
        [EmailAddress(ErrorMessage = "Por favor, insira um email v�lido.")]
        [MaxLength(320, ErrorMessage = "O campo de '{0}' s� pode conter no m�ximo 320 caracteres.")]
        public string Email { get; set; } = string.Empty;

        private readonly BancoDeDados Contexto;

        public BuscarEmailModel(BancoDeDados contexto)
        {
            Contexto = contexto;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPostBuscar() 
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(nameof(Email), "Email inv�lido.");
                return Page();
            }

            if (!Contexto.Usuarios.Any(x => x.Email == Email))
            {
                ModelState.AddModelError(nameof(Email), "N�o existe uma conta cadastrada com este email.");
                return Page();
            }

            return RedirectToPage("/Login/EsqueceuSenha/ConfirmarCodigo", new { Contexto.Usuarios.Where(x => x.Email == Email).First().Id });
        }
    }
}
