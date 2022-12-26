using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventzManager.Pages.Login.EsqueceuSenha
{
    public class ConfirmarCodigoModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPostConfirmar()
        {
            return RedirectToPage("/Login/EsqueceuSenha/CriarNovaSenha");
        }
    }
}
