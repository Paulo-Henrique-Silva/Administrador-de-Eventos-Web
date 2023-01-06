using EventzManager.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MimeKit;
using MailKit.Net.Smtp;

namespace EventzManager.Pages.Login.EsqueceuSenha
{
    public class ConfirmarCodigoModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "O campo de '{0}' � requerido.")]
        [DisplayName("C�digo de Seguran�a")]
        [MaxLength(6, ErrorMessage = "O campo de '{0}' s� pode conter no m�ximo 6 caracteres.")]
        public string CodigoSegurancaView { get; set; } = string.Empty;

        private readonly BancoDeDados Contexto;

        public ConfirmarCodigoModel(BancoDeDados contexto)
        {
            Contexto = contexto;
        }

        public void OnGet(uint id)
        {
            Usuario? usuario = Contexto.Usuarios.Find(id);

            if (usuario != null) //checa para caso a conta exista
            {
                TempData["email"] = usuario.Email;
                Response.Cookies.Append("id_usuario", id.ToString());

                try
                {
                    usuario.EmailFoiVerificado = false;
                    usuario.CodigoSeguranca = $"{new Random().Next(100):D2}{new Random().Next(100):D2}{new Random().Next(100):D2}";
                    Contexto.SaveChanges();

                    var emailObjeto = new MimeMessage();
                    var smtp = new SmtpClient();

                    //insira o seu email personalizado aqui
                    string email = "";
                    string senha = "";

                    //manda o email do c�digo ao usu�rio.
                    emailObjeto.From.Add(MailboxAddress.Parse(email));
                    emailObjeto.To.Add(MailboxAddress.Parse(usuario.Email));
                    emailObjeto.Subject = "Recupera��o de Senha - Eventz Manager";
                    emailObjeto.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = $"<h3>Ol�!</h3> <p>O c�digo de recupera��o �: <b>{usuario.CodigoSeguranca}</b>.</p> <p>Caso n�o tenha solicitado isso, apenas ignore o email.</p>" };

                    smtp.Connect("smtp.gmail.com", 465, true);
                    smtp.Authenticate(email, senha);
                    smtp.Send(emailObjeto);
                }
                catch (Exception ex)
                {
                    TempData["erro"] = ex.Message + "Um erro ocorreu. Tente novamente";
                }
            }
        }

        public void OnGetCodigoErrado(uint id)
        {
            Usuario? usuario = Contexto.Usuarios.Find(id);

            if (usuario != null) //checa para caso a conta exista
            {
                TempData["email"] = usuario.Email;
                Response.Cookies.Append("id_usuario", id.ToString());

                ModelState.AddModelError(nameof(CodigoSegurancaView), "C�digo incorreto.");
            }
        }

        public IActionResult OnPostConfirmar()
        {
            string? cookieId = Request.Cookies["id_usuario"];

            if (cookieId == null) //o id do usu�rio n�o foi armazenado e, portanto, n�o poder� salvar o evento.
                return RedirectToPage("/Index");

            Usuario? usuarioNoBd = Contexto.Usuarios.Find(uint.Parse(cookieId));

            if (usuarioNoBd == null) //se a conta n�o existe.
                return RedirectToPage("/Index");

            if (usuarioNoBd.CodigoSeguranca != CodigoSegurancaView || !ModelState.IsValid)
                return RedirectToPage("", "CodigoErrado", new { usuarioNoBd.Id });

            return RedirectToPage("/Login/EsqueceuSenha/CriarNovaSenha", new { usuarioNoBd.Id });
        }
    }
}
