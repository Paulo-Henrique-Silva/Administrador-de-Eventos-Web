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
                TempData["id_usuario"] = id.ToString();

                Response.Cookies.Append("id_usuario", id.ToString());
            }
        }

        public IActionResult OnPostAdicionar()
        {
            string? cookieId = Request.Cookies["id_usuario"];

            if (cookieId == null) //o id do usuário não foi armazenado e, portanto, não poderá salvar o evento.
                return RedirectToPage("/Index");

            uint id;
            Usuario? usuario;

            id = uint.Parse(cookieId.ToString());
            usuario = Contexto.Usuarios.Find(id);

            const int MAX_EVENTOS = 50;

            if (ModelState.IsValid && usuario != null && Contexto.Eventos.Where(x => x.UsuarioId == usuario.Id).Count() < MAX_EVENTOS)
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
            else if (!(Contexto.Eventos.Where(x => x.UsuarioId == usuario.Id).Count() < MAX_EVENTOS))
            {
                TempData["erro"] = $"Número máximo de eventos ({MAX_EVENTOS}) foi alcançado. Não é possível inserir mais.";
                return RedirectToPage("/Principal/Acoes/Adicionar", new { id });
            }

            TempData["erro"] += "Não foi possível adicionar. Tente novamente.";
            return RedirectToPage("/Principal/Acoes/Adicionar", new { id });
        }
    }
}
