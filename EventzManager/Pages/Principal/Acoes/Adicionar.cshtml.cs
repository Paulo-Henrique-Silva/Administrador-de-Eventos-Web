using EventzManager.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventzManager.Pages.Principal.Acoes
{
    public class AdicionarModel : PageModel
    {
        public Evento NovoEvento { get; set; } = new Evento();

        private readonly BancoDeDados Contexto;

        public AdicionarModel(BancoDeDados contexto)
        {
            Contexto = contexto;
        }

        public void OnGet(uint id)
        {
            Usuario? usuario = Contexto.Usuarios.Find(id);

            if (usuario != null) //checa para caso a conta exista
            {
                TempData["primeiro_nome"] = usuario.Nome[..usuario.Nome.IndexOf(' ')]; //obtém o primeiro nome do usuário
                TempData["id"] = id;
                NovoEvento.Usuario = usuario;
            }
        }

        public IActionResult OnPostAdicionar()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Contexto.Eventos.Add(NovoEvento);
                    Contexto.SaveChanges();

                    return RedirectToPage("/Principal/ListaEventos", new { Id = 34 });
                }
                catch (Exception ex)
                {
                    TempData["erro"] = ex.Message + "\nNão foi possível adicionar. Tente novamente.";
                }
            }

            return Page();
        }
    }
}
