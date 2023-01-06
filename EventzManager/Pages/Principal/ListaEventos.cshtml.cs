using EventzManager.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventzManager.Pages.Principal
{
    public class ListaEventosModel : PageModel
    {
        public List<Evento> EventosView { get; set; } = new List<Evento>();

        private readonly BancoDeDados Contexto;

        public ListaEventosModel(BancoDeDados contexto)
        {
            Contexto = contexto;
        }

        public void OnGet(uint id)
        {
            Usuario? usuario = Contexto.Usuarios.Find(id);

            if (usuario == null) //checa para caso a conta exista
                return;

            //dados a serem usados no header da página.
            TempData["id_usuario"] = id;

            Response.Cookies.Append("id_usuario", id.ToString());

            //obtém lista de eventos no bd
            List<Evento> eventosNoBd = Contexto.Eventos.Where(e => e.UsuarioId == id).OrderBy(x => x.Data).ToList();
            EventosView = eventosNoBd.ToList();

            //coloca os eventos que possuem a data expirada por último na lista.
            foreach (var evento in eventosNoBd)
            {
                if (evento.Data < DateTime.Now)
                {
                    EventosView.Remove(evento);
                    EventosView.Add(evento);
                }
            }
        }

        public IActionResult OnGetDeletar(uint id)
        {
            var cookieIdUsuario = Request.Cookies["id_usuario"];
            Evento? evento = Contexto.Eventos.Find(id);

            if (cookieIdUsuario == null)
                return RedirectToPage("/Login/Entrar");
            else if (evento == null)
                return RedirectToPage("/Principal/ListaEventos", new { Id = cookieIdUsuario });

            try
            {
                Contexto.Eventos.Remove(evento);
                Contexto.SaveChanges();
            }
            catch
            {
                return RedirectToPage("/Principal/ListaEventos", new { Id = cookieIdUsuario });
            }

            return RedirectToPage("/Principal/ListaEventos", new { Id = cookieIdUsuario });
        }
    }
}
