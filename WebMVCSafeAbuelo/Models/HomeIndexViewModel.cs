using WebMVCSafeAbuelo.Models;

namespace WebMVCSafeAbuelo.ViewModels
{
    public class HomeIndexViewModel
    {
        public int TotalReportes { get; set; }
        public int ReportesPendientes { get; set; }
        public int FraudesConfirmados { get; set; }

        // La lista de reportes que mostraremos en el feed
        public IEnumerable<ReporteIncidente> AlertasRecientes { get; set; } = new List<ReporteIncidente>();
    }
}