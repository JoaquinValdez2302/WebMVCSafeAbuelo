using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebMVCSafeAbuelo.Models;

namespace WebMVCSafeAbuelo.Data
{
    public class ApplicationDbContext : IdentityDbContext<UsuarioAdministrador>
    {
        public DbSet<WebMVCSafeAbuelo.Models.MetodologiaAtaque> MetodologiaAtaque { get; set; } = default!;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Agregamos tu modelo núcleo aquí para que EF lo transforme en tabla
        public DbSet<ReporteIncidente> ReportesIncidentes { get; set; }

        public DbSet<EvidenciaIncidente> EvidenciasIncidentes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Esto es vital si usas Identity, no lo borres
            base.OnModelCreating(modelBuilder);

            // Le decimos a EF Core: "Guarda la plataforma como un texto en la base de datos"
            modelBuilder.Entity<ReporteIncidente>()
                .Property(r => r.PlataformaDeContacto)
                .HasConversion<string>();

            // Le decimos a EF Core: "Guarda el tipo de evidencia como un texto en la base de datos"
            modelBuilder.Entity<EvidenciaIncidente>()
                .Property(e => e.Tipo)
                .HasConversion<string>();
        }
    }
}
