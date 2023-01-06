using EventzManager.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MimeKit;
using MailKit.Net.Smtp;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EventzManager.Pages.Login.Cadastro
{
    public class ConfirmarEmailModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "O campo de '{0}' é requerido.")]
        [DisplayName("Código de Segurança")]
        [MaxLength(6, ErrorMessage = "O campo de '{0}' só pode conter no máximo 6 caracteres.")]
        public string CodigoSegurancaView { get; set; } = string.Empty;

        private readonly BancoDeDados Contexto;

        public ConfirmarEmailModel(BancoDeDados contexto)
        {
            Contexto = contexto;
        }

        public void OnGet(uint id)
        {
            Usuario? usuarioNoBd = Contexto.Usuarios.Find(id);

            if (usuarioNoBd != null)
            {
                TempData["email"] = usuarioNoBd.Email;
                Response.Cookies.Append("id_usuario", id.ToString());

                var emailObjeto = new MimeMessage();
                var smtp = new SmtpClient();

                //insira o seu próprio email personalizado aqui
                string email = "";
                string senha = "";

                try
                {
                    usuarioNoBd.CodigoSeguranca = $"{new Random().Next(100):D2}{new Random().Next(100):D2}{new Random().Next(100):D2}";
                    Contexto.SaveChanges();

                    //manda o email do código ao usuário.
                    emailObjeto.From.Add(MailboxAddress.Parse(email));
                    emailObjeto.To.Add(MailboxAddress.Parse(usuarioNoBd.Email));
                    emailObjeto.Subject = "Código de verificação - Eventz Manager";
                    emailObjeto.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = $"<h3>Olá!</h3> <p>O código de verificação é: <b>{usuarioNoBd.CodigoSeguranca}</b>.</p> <p>Caso não tenha solicitado isso, apenas ignore o email.</p>" };

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
        }

        public void OnGetCodigoErrado(uint id)
        {
            Usuario? usuarioNoBd = Contexto.Usuarios.Find(id);

            if (usuarioNoBd != null)
            {
                TempData["email"] = usuarioNoBd.Email;
                ModelState.AddModelError(nameof(CodigoSegurancaView), "Código incorreto.");
            }
        }

        public IActionResult OnPostConfirmar()
        {
            string? cookieId = Request.Cookies["id_usuario"];

            if (cookieId == null) //o id do usuário não foi armazenado e, portanto, não poderá salvar o evento.
                return RedirectToPage("/Index");

            Usuario? usuarioNoBd = Contexto.Usuarios.Find(uint.Parse(cookieId));

            if (usuarioNoBd == null) //se a conta não existe.
                return RedirectToPage("/Index");

            if (usuarioNoBd.CodigoSeguranca == CodigoSegurancaView)
            {
                try
                {
                    usuarioNoBd.EmailFoiVerificado = true;
                    Contexto.SaveChanges();

                    return RedirectToPage("/Principal/ListaEventos", new { Id = cookieId });
                }
                catch (Exception ex)
                {
                    TempData["erro"] = ex.Message + "\nUm erro ocorreu. Por favor, tente novamente.";
                    return Page();
                }
            }

            return RedirectToPage("", "CodigoErrado", new { Id = cookieId });
        }
    }
}
