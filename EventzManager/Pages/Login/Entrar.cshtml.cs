using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventzManager.Pages.Login
{
    public class EntrarModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPostEntrar()
        {
            return RedirectToPage("/Principal/ListaEventos");
        }
    }
}
