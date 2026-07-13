using System.ComponentModel.DataAnnotations;

namespace WebMVCSafeAbuelo.Models
{
    public class PerfilUsuario
    {
        [Key]
        public string FirebaseUid { get; set; }

        [Required]
        [MaxLength(100)]
        public string NombreCompleto { get; set; }

        [MaxLength(20)]
        public string Telefono { get; set; }

        public string EmailContacto { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        // Propiedad de Navegación: Un perfil puede tener muchos reportes
        public ICollection<ReporteIncidente> Reportes { get; set; }
    }
}