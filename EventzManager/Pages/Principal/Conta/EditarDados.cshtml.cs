using EventzManager.Modelos;
using EventzManager.ViewsModelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EventzManager.Pages.Principal.Conta
{
    public class EditarDadosModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "O campo de '{0}' é requerido.")]
        [MaxLength(80, ErrorMessage = "O campo de '{0}' só pode conter no máximo 80 caracteres.")]
        public string NomeView { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "O campo de '{0}' é requerido.")]
        [EmailAddress(ErrorMessage = "Por favor, insira um email válido.")]
        [MaxLength(320, ErrorMessage = "O campo de '{0}' só pode conter no máximo 320 caracteres.")]
        public string EmailView { get; set; } = string.Empty;

        private readonly BancoDeDados Contexto;

        public EditarDadosModel(BancoDeDados contexto)
        {
            Contexto = contexto;
        }

        public void OnGet(uint id)
        {
            Usuario? usuario = Contexto.Usuarios.Find(id);

            if (usuario != null) //checa para caso a conta exista
            {
                TempData["id_usuario"] = id.ToString();

                NomeView = usuario.Nome;
                EmailView = usuario.Email;

                Response.Cookies.Append("id_usuario", id.ToString());
            }
        }

        public IActionResult OnPostEditar()
        {
            string? cookieId = Request.Cookies["id_usuario"];

            if (cookieId == null)
                return RedirectToPage("/Index");

            Usuario? usuarioNoBd = Contexto.Usuarios.Find(uint.Parse(cookieId.ToString()));

            if (usuarioNoBd == null)
                return RedirectToPage("/Index");

            if (usuarioNoBd.Email != EmailView && Contexto.Usuarios.Any(x => x.Email == EmailView))
            {
                TempData["erro"] = "Já existe uma conta cadastrada com este email.";
                return RedirectToPage("", new { id = cookieId });
            }

            if (ModelState.IsValid && (usuarioNoBd.Nome != NomeView || usuarioNoBd.Email != EmailView))
            {
                try
                {
                    usuarioNoBd.Nome = NomeView.ToUpper();

                    if (usuarioNoBd.Email != EmailView)
                    {
                        usuarioNoBd.Email = EmailView;
                        usuarioNoBd.EmailFoiVerificado = false;
                        Contexto.SaveChanges();

                        return RedirectToPage("/Login/Cadastro/ConfirmarEmail", new { id = cookieId });
                    }

                    Contexto.SaveChanges();
                    return RedirectToPage("/Principal/ListaEventos", new { id = cookieId });
                }
                catch (Exception ex) 
                {
                    TempData["erro"] = ex.Message + "Um erro ocorreu. Tente novamente.";
                }
            }

            return RedirectToPage("", new { id = cookieId });
        }
    }
}
