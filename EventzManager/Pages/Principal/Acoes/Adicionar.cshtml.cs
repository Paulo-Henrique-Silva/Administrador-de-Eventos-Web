using EventzManager.Modelos;
using EventzManager.ViewsModelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventzManager.Pages.Principal.Acoes
{
    public class AdicionarModel : PageModel
    {
        [BindProperty]
        public EventoView NovoEvento { get; set; } = new();

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
                TempData["id"] = id.ToString();
                TempData.Keep("id");
            }
        }

        public IActionResult OnPostAdicionar()
        {
            uint id = 0u;

            if (TempData.ContainsKey("id"))
                id = uint.Parse(TempData["id"].ToString());

            if (ModelState.IsValid)
            {
                Evento evento = new()
                {
                    Titulo = NovoEvento.Titulo,
                    Descricao = NovoEvento.Descricao,
                    Data = NovoEvento.Data,
                    Usuario = Contexto.Usuarios.Find(id),
                };

                try
                {
                    Contexto.Eventos.Add(evento);
                    Contexto.SaveChanges();

                    return RedirectToPage("/Principal/ListaEventos", new { id });
                }
                catch (Exception ex)
                {
                    TempData["erro"] = ex.Message;
                }
            }

            TempData["erro"] += "Não foi possível adicionar. Tente novamente.";
            return id != 0u ? RedirectToPage("/Principal/Acoes/Adicionar", new { id }) : RedirectToPage("/Index");
        }
    }
}
