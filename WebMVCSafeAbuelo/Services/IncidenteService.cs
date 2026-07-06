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

        public async Task<(IEnumerable<ReporteIncidente> Incidentes, int TotalPaginas)> ObtenerPaginadosAsync(string buscar, int pagina, int cantidadPorPagina)
        {
            var query = _context.ReportesIncidentes.AsQueryable();

            // 1. Si hay un texto de búsqueda, filtramos (por localidad, descripción o plataforma)
            if (!string.IsNullOrEmpty(buscar))
            {
                buscar = buscar.ToLower();
                // Nota: Convertimos a minúsculas para que la búsqueda sea más flexible
                query = query.Where(r =>
                    r.Localidad.ToString().ToLower().Contains(buscar) ||
                    r.DescripcionDelEngaño.ToLower().Contains(buscar) ||
                    r.PlataformaDeContacto.ToString().ToLower().Contains(buscar));
            }

            // 2. Calculamos el total de páginas
            int totalRegistros = await query.CountAsync();
            int totalPaginas = (int)Math.Ceiling(totalRegistros / (double)cantidadPorPagina);

            // Si la búsqueda no arroja resultados, evitamos que devuelva 0 páginas para que la UI no se rompa
            if (totalPaginas == 0) totalPaginas = 1;

            // 3. Aplicamos ordenamiento y paginación
            var incidentes = await query
                .OrderByDescending(r => r.FechaReporte) // Los más recientes primero
                .Skip((pagina - 1) * cantidadPorPagina)
                .Take(cantidadPorPagina)
                .ToListAsync();

            return (incidentes, totalPaginas);
        }
        public async Task<ReporteIncidente?> ObtenerPorIdAsync(int? id)
        {
            if (id == null) return null;
            return await _context.ReportesIncidentes.FindAsync(id);
        }

        // Dentro de tu clase IncidenteService.cs

        public async Task CrearAsync(ReporteIncidente reporte)
        {
            // 1. Iniciamos la transacción en la capa de datos
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Add(reporte);
                await _context.SaveChangesAsync();
                // 2. Si todo sale bien, confirmamos en firme
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                // 3. REVERSIÓN: Si la red falla, hacemos Rollback
                await transaction.RollbackAsync();

                // Lanzamos el error hacia arriba para que el Controlador lo atrape y muestre el mensaje visual
                throw;
            }
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