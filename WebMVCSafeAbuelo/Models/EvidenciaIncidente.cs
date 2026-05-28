using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebMVCSafeAbuelo.Models
{
    public class EvidenciaIncidente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Tipo { get; set; } = string.Empty; // Ej: "Teléfono", "URL", "CBU/CVU/Alias", "Email"

        [Required]
        [StringLength(255)]
        public string Valor { get; set; } = string.Empty; // Ej: "+543794123456", "alias.falso.mp", "http://link-clonado.com"

        [StringLength(150)]
        public string Notas { get; set; } = string.Empty; // Detalles adicionales sobre la evidencia

        // --- Relación con ReporteIncidente (Clave Foránea) ---
        [Required]
        public int ReporteIncidenteId { get; set; }

        [ForeignKey("ReporteIncidenteId")]
        public ReporteIncidente? ReporteIncidente { get; set; }
    }
}
