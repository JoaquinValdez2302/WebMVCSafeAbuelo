using WebMVCSafeAbuelo.Models;

namespace WebMVCSafeAbuelo.Services
{
    public interface IIncidenteService
    {
        Task<IEnumerable<ReporteIncidente>> ObtenerTodosAsync();
        Task<ReporteIncidente?> ObtenerPorIdAsync(int? id);
        Task CrearAsync(ReporteIncidente reporte);
        Task ActualizarAsync(ReporteIncidente reporte);
        Task EliminarAsync(int id);
        bool Existe(int? id);
    }
}