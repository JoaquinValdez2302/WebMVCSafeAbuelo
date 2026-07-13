using System.ComponentModel.DataAnnotations;

namespace WebMVCSafeAbuelo.DTOs
{
    public class ReporteAltaDto
    {
        [Required]
        public string AuthorId { get; set; } // El UUID de Firebase

        [Required]
        public string Provincia { get; set; }

        [Required]
        public string Localidad { get; set; }

        [Required]
        public string PlataformaDeContacto { get; set; }

        public string? PlataformaOtra { get; set; }

        [Required]
        public string DescripcionDelEngaño { get; set; }

        // Asumimos que al crearse, el estado es "Pendiente" por defecto en el backend
        // pero si el contrato exige que lo envíen, lo mapeamos aquí.
        public string? Estado { get; set; }

        public List<EvidenciaDto> Evidencias { get; set; } = new List<EvidenciaDto>();
    }
}