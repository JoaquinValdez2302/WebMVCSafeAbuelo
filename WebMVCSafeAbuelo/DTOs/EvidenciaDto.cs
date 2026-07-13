namespace WebMVCSafeAbuelo.DTOs
{
    public class EvidenciaDto
    {
        public int? Id { get; set; } // Opcional, útil para los GET
        public string Tipo { get; set; } // Será el string del Enum
        public string Valor { get; set; }
        public string Notas { get; set; }
        public string? LinkEvidencia { get; set; }
    }
}