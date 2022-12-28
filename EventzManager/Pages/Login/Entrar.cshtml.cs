using EventzManager.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventzManager.Pages.Login
{
    public class EntrarModel : PageModel
    {
        [BindProperty]
        public Usuario Usuario { get; set; } = new Usuario();

        private readonly BancoDeDados Contexto;

        public EntrarModel(BancoDeDados contexto)
        {
            Contexto = contexto;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPostEntrar()
        {
            if (!Contexto.Usuarios.Any(x => x.Email == Usuario.Email))
                ModelState.AddModelError("Usuario.Email", "Não existe uma conta cadastrada com este email.");
            else
            {
                Usuario usuarioQuery = Contexto.Usuarios.Where(x => x.Email == Usuario.Email).First();

                if (usuarioQuery.Senha != Usuario.Senha)
                    ModelState.AddModelError("Usuario.Senha", "Senha incorreta.");
                else if (!usuarioQuery.EmailFoiVerificado)
                    return RedirectToPage("/Login/Cadastro/ConfirmarEmail", new { usuarioQuery.Id });
                else
                    return RedirectToPage("/Principal/ListaEventos", new { usuarioQuery.Id });
            }

            return Page();
        }
    }
}
