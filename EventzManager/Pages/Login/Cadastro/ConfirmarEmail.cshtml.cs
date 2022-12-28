using EventzManager.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MimeKit;
using MailKit.Net.Smtp;

namespace EventzManager.Pages.Login.Cadastro
{
    public class ConfirmarEmailModel : PageModel
    {
        [BindProperty]
        public Usuario NovoUsuario { get; set; } = new Usuario();

        private readonly BancoDeDados Contexto;

        public ConfirmarEmailModel(BancoDeDados contexto)
        {
            Contexto = contexto;
        }

        public void OnGet(uint id)
        {
            Usuario? usuarioQuery = Contexto.Usuarios.Find(id);

            if (usuarioQuery != null)
            {
                NovoUsuario = usuarioQuery;

                var emailObjeto = new MimeMessage();
                var smtp = new SmtpClient();

                //insira o seu pr�prio email de personalizado aqui
                string email = "";
                string senha = "";

                try
                {
                    NovoUsuario.CodigoSeguranca = $"{new Random().Next(100):D2}{new Random().Next(100):D2}{new Random().Next(100):D2}";
                    Contexto.SaveChanges();

                    //manda o email do c�digo ao usu�rio.
                    emailObjeto.From.Add(MailboxAddress.Parse(email));
                    emailObjeto.To.Add(MailboxAddress.Parse(NovoUsuario.Email));
                    emailObjeto.Subject = "C�digo de verifica��o - Eventz Manager";
                    emailObjeto.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = $"<h3>Ol�!</h3> <p>O c�digo de verifica��o �: <b>{NovoUsuario.CodigoSeguranca}</b>.</p> <p>Caso n�o tenha solicitado isso, apenas ignore o email.</p>" };

                    smtp.Connect("smtp.gmail.com", 465, true);
                    smtp.Authenticate(email, senha);
                    smtp.Send(emailObjeto);
                }
                catch (Exception ex)
                {
                    TempData["erro"] = ex.Message + "\nUm erro ocorreu. Por favor, tente novamente.";
                }
                finally
                {
                    smtp.Disconnect(true);
                }
            }
            else
                VoltarAoCadastro();
        }

        public void OnGetCodigoErrado(uint id)
        {
            Usuario? usuarioQuery = Contexto.Usuarios.Find(id);

            if (usuarioQuery != null)
            {
                NovoUsuario = usuarioQuery;
                TempData["codigo"] = "C�digo incorreto.";
            }
            else
                VoltarAoCadastro();
        }

        public IActionResult OnPostConfirmar(string CodigoInserido)
        {
            if (CodigoInserido == NovoUsuario.CodigoSeguranca)
            {
                try
                {
                    Usuario? usuarioQuery = Contexto.Usuarios.Find(NovoUsuario.Id);

                    if (usuarioQuery == null)
                        throw new Exception("Usu�rio n�o encontrado.");

                    usuarioQuery.EmailFoiVerificado = true;
                    Contexto.SaveChanges();

                    return RedirectToPage("/Principal/ListaEventos", new { NovoUsuario.Id });
                }
                catch (Exception ex)
                {
                    TempData["erro"] = ex.Message + "\nUm erro ocorreu. Por favor, tente novamente.";
                    return Page();
                }
            }

            return RedirectToPage("", "CodigoErrado", new { NovoUsuario.Id });
        }

        private IActionResult VoltarAoCadastro() 
        { 
            return RedirectToPage("/Cadastro/PreecherDados"); 
        }
    }
}
