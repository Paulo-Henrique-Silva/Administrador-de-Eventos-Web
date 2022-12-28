using EventzManager.Modelos;
using EventzManager.Pages.Login.EsqueceuSenha;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace EventzManager.Pages.Login.Cadastro
{
    public class ConfirmarEmailModel : PageModel
    {
        [BindProperty]
        public Usuario NovoUsuario { get; set; } = new Usuario();

        private readonly BancoDeDados Contexto;

        public ConfirmarEmailModel(BancoDeDados contexto)
        {
            Contexto = contexto;
        }

        public void OnGet(uint id)
        {
            Usuario? usuarioQuery; //permite valores nulos
            usuarioQuery = Contexto.Usuarios.Find(id);

            if (usuarioQuery != null)
            {
                NovoUsuario = usuarioQuery;

                try
                {
                    NovoUsuario.CodigoSeguranca = $"{new Random().Next(100):D2}{new Random().Next(100):D2}{new Random().Next(100):D2}";
                    Contexto.SaveChanges();
                }
                catch (Exception ex)
                {
                    TempData["erro"] = ex.Message + "\nUm erro ocorreu. Por favor, tente novamente.";
                }
            }
            else
                VoltarAoCadastro();
        }

        public void OnGetCodigoErrado(uint id)
        {
            Usuario? usuarioQuery; //permite valores nulos
            usuarioQuery = Contexto.Usuarios.Find(id);

            if (usuarioQuery != null)
            {
                NovoUsuario = usuarioQuery;
                //TempData["erro"] = NovoUsuario.CodigoSeguranca;
            }
            else
                VoltarAoCadastro();
        }

        public IActionResult OnPostConfirmar(string CodigoInserido)
        {
            if (CodigoInserido != NovoUsuario.CodigoSeguranca)
            {
                TempData["erro"] = CodigoInserido + "-" + NovoUsuario.CodigoSeguranca;
            }

            return CodigoInserido == NovoUsuario.CodigoSeguranca ? RedirectToPage("/Principal/ListaEventos", new { NovoUsuario.Id }) : RedirectToPage("", "CodigoErrado", new { NovoUsuario.Id });
        }

        private IActionResult VoltarAoCadastro() 
        { 
            return RedirectToPage("/Cadastro/PreecherDados"); 
        }
    }
}
