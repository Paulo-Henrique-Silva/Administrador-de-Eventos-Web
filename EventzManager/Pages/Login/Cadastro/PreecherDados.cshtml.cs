using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventzManager.Pages.Login.Cadastro
{
    public class PreecherDadosModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPostCadastrar()
        {
            return RedirectToPage("/Login/Cadastro/ConfirmarEmail");
        }
    }
}
