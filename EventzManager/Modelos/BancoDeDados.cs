using Microsoft.EntityFrameworkCore;

namespace EventzManager.Modelos
{
    public class BancoDeDados : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Evento> Eventos { get; set; }

        public BancoDeDados(DbContextOptions<BancoDeDados> options) : base(options)
        {

        }
    }
}
