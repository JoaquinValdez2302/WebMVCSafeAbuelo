namespace WebMVCSafeAbuelo.DTOs
{
    public class ReporteDetalleDto : ReporteResumenDto
    {
        public string PlataformaDeContacto { get; set; }
        public string? PlataformaOtra { get; set; }
        public bool EjercePresionPsicologica { get; set; }
        public bool GeneraSentidoDeUrgencia { get; set; }
        public string DescripcionDelEngaño { get; set; }

        public List<EvidenciaDto> Evidencias { get; set; } = new List<EvidenciaDto>();
    }
}