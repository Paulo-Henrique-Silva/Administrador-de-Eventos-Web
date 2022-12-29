using EventzManager.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventzManager.Pages.Principal.Acoes
{
    public class AdicionarModel : PageModel
    {
        [BindProperty]
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
                //NovoEvento.Titulo = "LSUIS";
                //NovoEvento.Descricao = "sadsad";
                //NovoEvento.Data = DateTime.Today;

                //Contexto.Eventos.Add(NovoEvento);
                //Contexto.SaveChanges();
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

                    var id = TempData["id"];
                    return RedirectToPage("/Principal/ListaEventos", new { id });
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
