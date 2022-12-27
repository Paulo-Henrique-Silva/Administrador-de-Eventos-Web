using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EventzManager.Modelos
{
    [Table("tb_usuarios")]
    [Index(nameof(Email), IsUnique = true)]
    public class Usuario
    {
        [Column("id")]
        [Key]
        [Required]
        public uint Id { get; set; }

        [Column("nome")]
        [DisplayName("Nome")]
        [Required(ErrorMessage = "O campo de 'nome' é requerido.")]
        [MaxLength(80, ErrorMessage = "O campo de 'nome' só pode conter no máximo 80 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Column("email")]
        [DisplayName("Email")]
        [Required(ErrorMessage = "O campo de 'email' é requerido.")]
        [MaxLength(320, ErrorMessage = "O campo de 'email' só pode conter no máximo 320 caracteres.")]
        public string Email { get; set; } = string.Empty;
        
        [Column("senha")]
        [DisplayName("Senha")]
        [Required(ErrorMessage = "O campo de 'senha' é requerido.")]
        [MaxLength(60, ErrorMessage = "O campo de 'senha' só pode conter no máximo 60 caracteres.")]
        public string Senha { get; set; } = string.Empty;
    }
}
