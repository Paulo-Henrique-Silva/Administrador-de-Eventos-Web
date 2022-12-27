using EventzManager.Modelos;
using EventzManager.Pages.Login.EsqueceuSenha;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace EventzManager.Pages.Login.Cadastro
{
    public class ConfirmarEmailModel : PageModel
    {
        public Usuario NovoUsario { get; set; } = new Usuario();

        public readonly string Codigo = $"{new Random().Next(100):D2}{new Random().Next(100):D2}{new Random().Next(100):D2}";

        public void OnGet(Usuario novoUsuario)
        {
            NovoUsario = novoUsuario;
        }

        public IActionResult OnPostConfirmar(string CodigoInserido)
        {
            if (CodigoInserido != Codigo)
            {
                ModelState.AddModelError("Codigo", "Código incorreto.");
            }

            if (ModelState.IsValid)
                return RedirectToPage("/Principal/ListaEventos");

            return Page();
        }
    }
}
