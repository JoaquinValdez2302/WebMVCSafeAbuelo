using Microsoft.EntityFrameworkCore;
using WebMVCSafeAbuelo.Data;
using WebMVCSafeAbuelo.Models;
using WebMVCSafeAbuelo.Models.Enums;
using WebMVCSafeAbuelo.DTOs;

namespace WebMVCSafeAbuelo.Services
{
    public class IncidenteService : IIncidenteService
    {
        private readonly ApplicationDbContext _context;

        public IncidenteService(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================================================================
        // MÉTODOS PARA LA WEB MVC 
        // =========================================================================

        public async Task<IEnumerable<ReporteIncidente>> ObtenerTodosAsync()
        {
            return await _context.ReportesIncidentes.ToListAsync();
        }

        public async Task<(IEnumerable<ReporteIncidente> Incidentes, int TotalPaginas)> ObtenerPaginadosAsync(string buscar, int pagina, int cantidadPorPagina)
        {
            var query = _context.ReportesIncidentes.AsQueryable();

            if (!string.IsNullOrEmpty(buscar))
            {
                buscar = buscar.ToLower();
                query = query.Where(r =>
                    r.Localidad.ToString().ToLower().Contains(buscar) ||
                    r.DescripcionDelEngaño.ToLower().Contains(buscar) ||
                    r.PlataformaDeContacto.ToString().ToLower().Contains(buscar));
            }

            int totalRegistros = await query.CountAsync();
            int totalPaginas = (int)Math.Ceiling(totalRegistros / (double)cantidadPorPagina);

            if (totalPaginas == 0) totalPaginas = 1;

            var incidentes = await query
                .OrderByDescending(r => r.FechaReporte)
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

        public async Task CrearAsync(ReporteIncidente reporte)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Add(reporte);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
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


        // =========================================================================
        // MÉTODOS PARA LA API MÓVIL 
        // =========================================================================

        public async Task<IEnumerable<ReporteDetalleDto>> ObtenerReportesAceptadosPaginadosAsync(int page, int pageSize)
        {
            return await _context.ReportesIncidentes
                .Include(r => r.Evidencias) // <-- ¡Clave para que el array de evidencias no llegue vacío!
                .Where(r => r.Estado == EstadoReporte.Aceptado)
                .OrderByDescending(r => r.FechaReporte)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new ReporteDetalleDto
                {
                    Id = r.Id,
                    Author = r.AuthorId,
                    DateTime = r.FechaReporte,
                    Provincia = r.Provincia.ToString(),
                    Localidad = r.Localidad.ToString(),
                    Estado = r.Estado.ToString(),

                    // --- Los datos detallados que ahora sí viajarán a la App ---
                    PlataformaDeContacto = r.PlataformaDeContacto.ToString(),
                    PlataformaOtra = r.PlataformaOtra,
                    EjercePresionPsicologica = r.EjercePresionPsicologica,
                    GeneraSentidoDeUrgencia = r.GeneraSentidoDeUrgencia,
                    DescripcionDelEngaño = r.DescripcionDelEngaño,
                    Evidencias = r.Evidencias.Select(e => new EvidenciaDto
                    {
                        Id = e.Id,
                        Tipo = e.Tipo.ToString(),
                        Valor = e.Valor,
                        Notas = e.Notas,
                        LinkEvidencia = e.LinkEvidencia
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<ReporteDetalleDto?> ObtenerReportePorIdAsync(int id)
        {
            return await _context.ReportesIncidentes
                .Include(r => r.Evidencias) // Join anidado esencial para traer las evidencias
                .Where(r => r.Id == id)
                .Select(r => new ReporteDetalleDto
                {
                    Id = r.Id,
                    Author = r.AuthorId,
                    DateTime = r.FechaReporte,
                    Provincia = r.Provincia.ToString(),
                    Localidad = r.Localidad.ToString(),
                    Estado = r.Estado.ToString(),
                    PlataformaDeContacto = r.PlataformaDeContacto.ToString(),
                    PlataformaOtra = r.PlataformaOtra,
                    EjercePresionPsicologica = r.EjercePresionPsicologica,
                    GeneraSentidoDeUrgencia = r.GeneraSentidoDeUrgencia,
                    DescripcionDelEngaño = r.DescripcionDelEngaño,
                    Evidencias = r.Evidencias.Select(e => new EvidenciaDto
                    {
                        Id = e.Id,
                        Tipo = e.Tipo.ToString(),
                        Valor = e.Valor,
                        Notas = e.Notas,
                        LinkEvidencia = e.LinkEvidencia
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<ReporteDetalleDto> CrearReporteDesdeApiAsync(ReporteAltaDto dto)
        {
            // Limpieza de strings para parseo seguro de Enums (quita espacios y tildes si las envían desde la app)
            string CleanForEnum(string val) => val.Replace(" ", "").Replace("ó", "o").Replace("í", "i");

            var nuevoReporte = new ReporteIncidente
            {
                AuthorId = dto.AuthorId,
                FechaReporte = DateTime.UtcNow,
                Provincia = Enum.Parse<Provincia>(CleanForEnum(dto.Provincia), true),
                Localidad = Enum.Parse<Localidad>(CleanForEnum(dto.Localidad), true),
                PlataformaDeContacto = Enum.Parse<PlataformaDeContacto>(CleanForEnum(dto.PlataformaDeContacto), true),
                PlataformaOtra = dto.PlataformaOtra,
                DescripcionDelEngaño = dto.DescripcionDelEngaño,
                Estado = EstadoReporte.Pendiente, // Todo reporte nuevo nace pendiente por diseño

                // Mapeo del arreglo de evidencias
                Evidencias = dto.Evidencias.Select(e => new EvidenciaIncidente
                {
                    Tipo = Enum.Parse<TipoEvidencia>(CleanForEnum(e.Tipo), true),
                    Valor = e.Valor,
                    Notas = e.Notas,
                    LinkEvidencia = e.LinkEvidencia
                }).ToList()
            };

            _context.ReportesIncidentes.Add(nuevoReporte);
            await _context.SaveChangesAsync();

            // Reutilizamos el método de lectura detallada para devolver el objeto recién creado con su ID
            var reporteCreado = await ObtenerReportePorIdAsync(nuevoReporte.Id);
            return reporteCreado!;
        }

        public async Task<IEnumerable<ReporteDetalleDto>> ObtenerReportesPorUsuarioAsync(string userId)
        {
            return await _context.ReportesIncidentes
                .Include(r => r.Evidencias)
                .Where(r => r.AuthorId == userId) // Trae los del usuario, sin importar si están pendientes o aceptados
                .Select(r => new ReporteDetalleDto
                {
                    Id = r.Id,
                    Author = r.AuthorId,
                    DateTime = r.FechaReporte,
                    Provincia = r.Provincia.ToString(),
                    Localidad = r.Localidad.ToString(),
                    Estado = r.Estado.ToString(),
                    PlataformaDeContacto = r.PlataformaDeContacto.ToString(),
                    PlataformaOtra = r.PlataformaOtra,
                    EjercePresionPsicologica = r.EjercePresionPsicologica,
                    GeneraSentidoDeUrgencia = r.GeneraSentidoDeUrgencia,
                    DescripcionDelEngaño = r.DescripcionDelEngaño,
                    Evidencias = r.Evidencias.Select(e => new EvidenciaDto
                    {
                        Id = e.Id,
                        Tipo = e.Tipo.ToString(),
                        Valor = e.Valor,
                        Notas = e.Notas,
                        LinkEvidencia = e.LinkEvidencia
                    }).ToList()
                })
                .ToListAsync();
        }
    }
}