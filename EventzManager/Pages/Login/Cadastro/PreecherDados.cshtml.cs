using EventzManager.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EventzManager.Pages.Login.Cadastro
{
    public class PreecherDadosModel : PageModel
    {
        [BindProperty]
        public Usuario NovoUsuario { get; set; } = new Usuario();

        private readonly BancoDeDados Contexto;

        public PreecherDadosModel(BancoDeDados contexto)
        {
            Contexto = contexto; //obtem bd do projeto
        }

        public void OnGet()
        {
            
        }

        public IActionResult OnPostCadastrar()
        {
            if (Contexto.Usuarios.Any(x => x.Email == NovoUsuario.Email))
            {
                ModelState.AddModelError("NovoUsuario.Email", "Este email j� est� cadastrado.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Contexto.Usuarios.Add(NovoUsuario);
                    Contexto.SaveChanges();
                    return RedirectToPage("/Login/Cadastro/ConfirmarEmail", new { NovoUsuario.Id });
                }
                catch (Exception ex)
                {
                    TempData["erro"] = ex.Message + "\nN�o foi poss�vel cadastrar. Tente novamente.";
                }
            }

            return Page(); //atualiza a p�gina novamente caso a valida��o n�o se suceda
        }
    }
}
