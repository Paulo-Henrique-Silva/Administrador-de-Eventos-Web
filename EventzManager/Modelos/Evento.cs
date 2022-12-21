using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EventzManager.Modelos
{
    [Table("tb_eventos")]
    public class Evento
    {
        [Column("id")]
        [Key]
        [Required]
        public uint Id { get; set; }

        [Column("usuario_id")]
        [Required]
        public Usuario UsuarioId { get; set; } = new Usuario();

        [Column("titulo")]
        [DisplayName("Título")]
        [Required(ErrorMessage = "O campo de 'título' é requerido.")]
        [MaxLength(80, ErrorMessage = "O campo de 'título' só pode conter no máximo 80 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [Column("descricao")]
        [DisplayName("Descrição")]
        [MaxLength(200, ErrorMessage = "O campo de 'descrição' só pode conter no máximo 200 caracteres.")]
        public string? Descricao { get; set; }

        [Column("data")]
        [DisplayName("Data")]
        [Required(ErrorMessage = "O campo de 'data' é requerido.")]
        public DateTime Data { get; set; }
    }
}
