using EventzManager.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR.Protocol;
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

        public IActionResult OnPostCadastrar(string confirmacaoSenha)
        {
            if (Contexto.Usuarios.Any(x => x.Email == NovoUsuario.Email))
                ModelState.AddModelError("NovoUsuario.Email", "Este email já está cadastrado.");
            else if (confirmacaoSenha != NovoUsuario.Senha)
                ModelState.AddModelError("NovoUsuario.Senha", "Os campos de senha e confirmar senha precisam ser iguais.");

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
                    TempData["erro"] = ex.Message + "\nNão foi possível cadastrar. Tente novamente.";
                }
            }

            return Page(); //atualiza a página novamente caso a validação não se suceda
        }
    }
}
