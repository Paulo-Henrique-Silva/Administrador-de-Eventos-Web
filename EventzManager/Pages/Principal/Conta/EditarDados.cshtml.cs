using EventzManager.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventzManager.Pages.Principal.Conta
{
    public class EditarDadosModel : PageModel
    {
        private readonly BancoDeDados Contexto;

        public EditarDadosModel(BancoDeDados contexto)
        {
            Contexto = contexto;
        }

        public void OnGet(uint id)
        {
            Usuario? usuario = Contexto.Usuarios.Find(id);

            if (usuario != null) //checa para caso a conta exista
            {
                TempData["primeiro_nome"] = usuario.Nome[..usuario.Nome.IndexOf(' ')]; //obtém o primeiro nome do usuário
                TempData["id_usuario"] = id.ToString();

                Response.Cookies.Append("id_usuario", id.ToString());
            }
        }
    }
}
