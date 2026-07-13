using WebMVCSafeAbuelo.Models;
using WebMVCSafeAbuelo.DTOs;

namespace WebMVCSafeAbuelo.Services
{
    public interface IIncidenteService
    {
        // Métodos web MVC
        Task<IEnumerable<ReporteIncidente>> ObtenerTodosAsync();
        Task<(IEnumerable<ReporteIncidente> Incidentes, int TotalPaginas)> ObtenerPaginadosAsync(string buscar, int pagina, int cantidadPorPagina);
        Task<ReporteIncidente?> ObtenerPorIdAsync(int? id);
        Task CrearAsync(ReporteIncidente reporte);
        Task ActualizarAsync(ReporteIncidente reporte);
        Task EliminarAsync(int id);
        bool Existe(int? id);

        // Métodos para la API Móvil
        Task<IEnumerable<ReporteDetalleDto>> ObtenerReportesAceptadosPaginadosAsync(int page, int pageSize);
        Task<ReporteDetalleDto?> ObtenerReportePorIdAsync(int id);
        Task<ReporteDetalleDto> CrearReporteDesdeApiAsync(ReporteAltaDto dto);
        Task<IEnumerable<ReporteDetalleDto>> ObtenerReportesPorUsuarioAsync(string userId);
    }
}