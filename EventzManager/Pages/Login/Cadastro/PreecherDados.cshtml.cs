using EventzManager.Modelos;
using EventzManager.ViewsModelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EventzManager.Pages.Login.Cadastro
{
    public class PreecherDadosModel : PageModel
    {
        [BindProperty]
        public UsuarioView NovoUsuarioView { get; set; } = new();

        [BindProperty]
        [DisplayName("Confirmar senha")]
        [Required(ErrorMessage = "O campo de '{0}' é requerido.")]
        [MaxLength(60, ErrorMessage = "O campo de '{0}' só pode conter no máximo 60 caracteres.")]
        public string ConfirmacaoSenha { get; set; } = string.Empty;

        private readonly BancoDeDados Contexto;

        public PreecherDadosModel(BancoDeDados contexto)
        {
            Contexto = contexto; //obtem bd do projeto
        }

        public void OnGet()
        {
            
        }

        public IActionResult OnPostCadastrar()
        {
            if (Contexto.Usuarios.Any(x => x.Email == NovoUsuarioView.Email))
                ModelState.AddModelError("NovoUsuarioView.Email", "Este email já está cadastrado.");
            else if (ConfirmacaoSenha != NovoUsuarioView.Senha)
                ModelState.AddModelError("NovoUsuarioView.Senha", "Os campos de senha e confirmar senha precisam ser iguais.");

            if (ModelState.IsValid)
            {
                try
                {
                    Usuario novoUsuario = new()
                    {
                        Nome = NovoUsuarioView.Nome.ToUpper(),
                        Email = NovoUsuarioView.Email,
                        Senha = NovoUsuarioView.Senha,
                    };

                    Contexto.Usuarios.Add(novoUsuario);
                    Contexto.SaveChanges();
                    return RedirectToPage("/Login/Cadastro/ConfirmarEmail", new { novoUsuario.Id });
                }
                catch (Exception ex)
                {
                    TempData["erro"] = ex.Message + "\nNão foi possível cadastrar. Tente novamente.";
                }
            }

            return Page(); //atualiza a página novamente caso a validação não se suceda
        }
    }
}
