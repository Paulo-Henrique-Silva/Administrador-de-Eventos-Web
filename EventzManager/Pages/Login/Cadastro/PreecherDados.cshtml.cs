using EventzManager.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EventzManager.Pages.Login.Cadastro
{
    public class PreecherDadosModel : PageModel
    {
        [BindProperty]
        public Usuario NovoUsario { get; set; } = new Usuario();

        public void OnGet()
        {

        }

        public IActionResult OnPostCadastrar()
        {
            if (ModelState.IsValid)
                return RedirectToPage("/Login/Cadastro/ConfirmarEmail", NovoUsario);

            return Page(); //atualiza a página novamente caso a validação não se suceda
        }
    }
}
