using Microsoft.EntityFrameworkCore;
using WebMVCSafeAbuelo.Data;
using WebMVCSafeAbuelo.Models;

namespace WebMVCSafeAbuelo.Services
{
    public class IncidenteService : IIncidenteService
    {
        private readonly ApplicationDbContext _context;

        public IncidenteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReporteIncidente>> ObtenerTodosAsync()
        {
            return await _context.ReportesIncidentes.ToListAsync();
        }

        public async Task<ReporteIncidente?> ObtenerPorIdAsync(int? id)
        {
            if (id == null) return null;
            return await _context.ReportesIncidentes.FindAsync(id);
        }

        public async Task CrearAsync(ReporteIncidente reporte)
        {
            // Ajuste de zona horaria necesario para Neon/PostgreSQL
            reporte.FechaReporte = DateTime.SpecifyKind(reporte.FechaReporte, DateTimeKind.Utc);

            _context.Add(reporte);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(ReporteIncidente reporte)
        {
            _context.Update(reporte);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var reporte = await _context.ReportesIncidentes.FindAsync(id);
            if (reporte != null)
            {
                _context.ReportesIncidentes.Remove(reporte);
                await _context.SaveChangesAsync();
            }
        }

        public bool Existe(int? id)
        {
            return _context.ReportesIncidentes.Any(e => e.Id == id);
        }
    }
}