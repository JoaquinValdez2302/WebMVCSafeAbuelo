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
    }
}
