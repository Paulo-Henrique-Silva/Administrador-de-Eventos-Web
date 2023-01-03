using EventzManager.Modelos;
using EventzManager.ViewsModelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventzManager.Pages.Principal.Acoes
{
    public class EditarModel : PageModel
    {
        [BindProperty]
        public EventoView Evento { get; set; } = new();

        private readonly BancoDeDados Contexto;

        public EditarModel(BancoDeDados contexto)
        {
            Contexto = contexto;
        }

        public void OnGet(uint idUsuario, uint idEvento)
        {
            Usuario? usuario = Contexto.Usuarios.Find(idUsuario);
            Evento? evento = Contexto.Eventos.Find(idEvento);

            if (usuario != null && evento != null) //checa para caso a conta e o evento existam
            {
                //obtem temp data as serem usadas no header
                TempData["primeiro_nome"] = usuario.Nome[..usuario.Nome.IndexOf(' ')];
                TempData["id_usuario"] = idUsuario.ToString();

                Evento.Titulo = evento.Titulo;
                Evento.Descricao = evento.Descricao;
                Evento.Data = evento.Data;

                //salva cookies para ser usado posteriormente.
                Response.Cookies.Append("id_usuario", idUsuario.ToString());
                Response.Cookies.Append("id_evento", idEvento.ToString());
            }
        }

        public IActionResult OnPost()
        {
            string? cookieIdUsuario = Request.Cookies["id_usuario"];
            string? cookieIdEvento = Request.Cookies["id_evento"];

            if (cookieIdUsuario == null || cookieIdEvento == null) //os ids não foram armazenados e, portanto, não poderá salvar a mudança.
                return RedirectToPage("/Index");

            Usuario? usuario = Contexto.Usuarios.Find(uint.Parse(cookieIdUsuario.ToString()));
            Evento? eventoASerAlterado = Contexto.Eventos.Find(uint.Parse(cookieIdEvento.ToString()));

            if (usuario == null || eventoASerAlterado == null) //se a conta não existe
                return RedirectToPage("/Index");

            try
            {
                eventoASerAlterado.Titulo = Evento.Titulo;
                eventoASerAlterado.Descricao = Evento.Descricao;
                eventoASerAlterado.Data = Evento.Data;
                Contexto.SaveChanges();

                return RedirectToPage("/Principal/ListaEventos", new { Id = cookieIdUsuario });
            }
            catch(Exception ex)
            {
                TempData["erro"] = ex.Message;
            }

            TempData["erro"] += "Não foi possível adicionar. Tente novamente.";
            return RedirectToPage("/Principal/Acoes/Editar", new { Id = cookieIdUsuario });
        }
    }
}
