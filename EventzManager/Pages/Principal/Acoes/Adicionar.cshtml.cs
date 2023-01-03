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

                Response.Cookies.Append("id", id.ToString());
            }
        }

        public IActionResult OnPostAdicionar()
        {
            string? cookieId = Request.Cookies["id"];

            if (cookieId == null) //o id do usuário não foi armazenado e, portanto, não poderá salvar o evento.
                return RedirectToPage("/Index");

            uint id;
            Usuario? usuario;

            id = uint.Parse(cookieId.ToString());
            usuario = Contexto.Usuarios.Find(id);

            if (ModelState.IsValid && usuario != null)
            {
                Evento evento = new()
                {
                    Titulo = NovoEvento.Titulo,
                    Descricao = NovoEvento.Descricao,
                    Data = NovoEvento.Data,
                    Usuario = usuario,
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
            else if (usuario == null)
                return RedirectToPage("/Index");

            TempData["erro"] += "Não foi possível adicionar. Tente novamente.";
            return RedirectToPage("/Principal/Acoes/Adicionar", new { id });
        }
    }
}
