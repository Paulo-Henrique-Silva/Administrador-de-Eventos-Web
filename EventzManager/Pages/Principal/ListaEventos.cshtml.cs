using EventzManager.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventzManager.Pages.Principal
{
    public class ListaEventosModel : PageModel
    {
        public List<Evento> Eventos { get; set; } = new List<Evento>();

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

            TempData["primeiro_nome"] = usuario.Nome[..usuario.Nome.IndexOf(' ')]; //obtém o primeiro nome do usuário
            TempData["id"] = id;

            Response.Cookies.Append("id-usuario", id.ToString());

            //obtém lista de eventos
            List<Evento> eventos = Contexto.Eventos.Where(e => e.UsuarioId == id).OrderBy(x => x.Data).ToList();
            Eventos = eventos.ToList();

            //coloca os eventos que possuem a data expirada por último na lista.
            foreach (var evento in eventos)
            {
                if (evento.Data < DateTime.Now)
                {
                    Eventos.Remove(evento);
                    Eventos.Add(evento);
                }
            }
        }

        public IActionResult OnGetDeletar(uint id)
        {
            var cookieIdUsuario = Request.Cookies["id-usuario"];
            Evento? evento = Contexto.Eventos.Find(id);

            if (cookieIdUsuario == null)
                return RedirectToPage("/Login/Entrar");
            else if (evento == null)
                return RedirectToPage("/Principal/ListaEventos", new { Id = uint.Parse(cookieIdUsuario.ToString()) });

            try
            {
                Contexto.Eventos.Remove(evento);
                Contexto.SaveChanges();
            }
            catch
            {
                return RedirectToPage("/Principal/ListaEventos", new { Id = uint.Parse(cookieIdUsuario.ToString()) });
            }

            return RedirectToPage("/Principal/ListaEventos", new { Id = uint.Parse(cookieIdUsuario.ToString()) });
        }
    }
}
