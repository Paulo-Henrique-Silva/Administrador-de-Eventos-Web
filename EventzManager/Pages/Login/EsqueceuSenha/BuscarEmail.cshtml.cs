using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventzManager.Pages.Login.EsqueceuSenha
{
    public class BuscarEmailModel : PageModel
    {
        public void OnGet()
        {

        }

        public IActionResult OnPostBuscar() 
        {
            return RedirectToPage("/Login/EsqueceuSenha/ConfirmarCodigo");
        }
    }
}
