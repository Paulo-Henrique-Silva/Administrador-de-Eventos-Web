using EventzManager.Modelos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EventzManager.ViewsModelos
{
    /// <summary>
    /// ViewModel para manipulação de formulários na page view da classe db "Evento".
    /// </summary>
    public class EventoView
    {
        [DisplayName("Título")]
        [Required(ErrorMessage = "O campo de '{0}' é requerido.")]
        [MaxLength(80, ErrorMessage = "O campo de '{0}' só pode conter no máximo 80 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [DisplayName("Descrição")]
        [MaxLength(200, ErrorMessage = "O campo de '{0}' só pode conter no máximo 200 caracteres.")]
        public string? Descricao { get; set; }

        [DisplayName("Data")]
        [Required(ErrorMessage = "O campo de '{0}' é requerido.")]
        public DateTime Data { get; set; }
    }
}