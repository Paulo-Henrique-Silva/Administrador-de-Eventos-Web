using EventzManager.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EventzManager.Pages.Principal.Conta
{
    public class ExcluirContaModel : PageModel
    {
        [BindProperty]
        [DisplayName("Senha")]
        [Required(ErrorMessage = "O campo de '{0}' é requerido.")]
        [MaxLength(60, ErrorMessage = "O campo de '{0}' só pode conter no máximo 60 caracteres.")]
        public string SenhaView { get; set; } = string.Empty;

        private readonly BancoDeDados Contexto;

        public ExcluirContaModel(BancoDeDados contexto)
        {
            Contexto = contexto;
        }

        public void OnGet(uint id)
        {
            if (Contexto.Usuarios.Find(id) != null) //checa para caso a conta exista
            {
                TempData["id_usuario"] = id.ToString();
                Response.Cookies.Append("id_usuario", id.ToString());
            }
        }

        public IActionResult OnPostExcluir()
        {
            string? cookieId = Request.Cookies["id_usuario"];

            if (cookieId == null) 
                return RedirectToPage("/Index");

            Usuario? usuarioNoBd = Contexto.Usuarios.Find(uint.Parse(cookieId.ToString()));

            if (usuarioNoBd == null)
                return RedirectToPage("/Index");

            if (usuarioNoBd.Senha != SenhaView)
            {
                TempData["erro"] = "Senha incorreta.";
                return RedirectToPage("", new { Id = cookieId });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Contexto.Usuarios.Remove(usuarioNoBd);
                    Contexto.SaveChanges();

                    return RedirectToPage("/Login/Entrar");
                }
                catch (Exception ex)
                {
                    TempData["erro"] = ex.Message + "Um erro ocorreu. Tente novamente.";
                }
            }

            return RedirectToPage("", new { Id = cookieId });
        }
    }
}
