using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebMVCSafeAbuelo.Models
{
    public enum TipoEvidencia
    {
        CapturaDePantalla,
        EnlaceMalicioso,
        NumeroDeTelefono,
        ComprobanteFinanciero,
        DocumentoFalso
    }
    public class EvidenciaIncidente
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe especificar el tipo de evidencia.")]
        public TipoEvidencia Tipo { get; set; }

        [Required(ErrorMessage = "Ingrese el valor apropiado.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "El valor debe tener entre 5 y 100 caracteres.")]
        [Display(Name = "Valor")]
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
