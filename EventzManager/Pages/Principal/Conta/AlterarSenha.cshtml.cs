using EventzManager.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EventzManager.Pages.Principal.Conta
{
    public class AlterarSenhaModel : PageModel
    {
        [BindProperty]
        [DisplayName("Senha atual")]
        [Required(ErrorMessage = "O campo de '{0}' é requerido.")]
        [MaxLength(60, ErrorMessage = "O campo de '{0}' só pode conter no máximo 60 caracteres.")]
        public string SenhaAtualView { get; set; } = string.Empty;

        [BindProperty]
        [DisplayName("Nova senha")]
        [Required(ErrorMessage = "O campo de '{0}' é requerido.")]
        [MaxLength(60, ErrorMessage = "O campo de '{0}' só pode conter no máximo 60 caracteres.")]
        public string NovaSenhaView { get; set; } = string.Empty;

        [BindProperty]
        [DisplayName("Confirmar senha")]
        [Required(ErrorMessage = "O campo de '{0}' é requerido.")]
        [MaxLength(60, ErrorMessage = "O campo de '{0}' só pode conter no máximo 60 caracteres.")]
        public string ConfirmacaoNovaSenhaView { get; set; } = string.Empty;

        private readonly BancoDeDados Contexto;

        public AlterarSenhaModel(BancoDeDados contexto)
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

        public IActionResult OnPostAlterar()
        {
            string? cookieId = Request.Cookies["id_usuario"];

            if (cookieId == null)
                return RedirectToPage("/Index");

            Usuario? usuarioNoBd = Contexto.Usuarios.Find(uint.Parse(cookieId.ToString()));

            if (usuarioNoBd == null)
                return RedirectToPage("/Index");

            if (usuarioNoBd.Senha != SenhaAtualView)
            {
                TempData["erro"] = "Senha incorreta.";
                return RedirectToPage("", new { Id = cookieId });
            }

            if (NovaSenhaView != ConfirmacaoNovaSenhaView)
            {
                TempData["erro"] = "Os campos de nova senha e confirmar nova senha precisam ser iguais.";
                return RedirectToPage("", new { Id = cookieId });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    usuarioNoBd.Senha = NovaSenhaView;
                    Contexto.SaveChanges();

                    return RedirectToPage("/Principal/ListaEventos", new { Id = cookieId });
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
