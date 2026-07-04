using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMVCSafeAbuelo.Data;
using WebMVCSafeAbuelo.Models;

namespace WebMVCSafeAbuelo.Controllers
{
    // Esta ruta significa que accederás a la API desde: tusitio.com/api/MetodologiasApi
    [Route("api/[controller]")]
    [ApiController]
    public class MetodologiasApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MetodologiasApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/MetodologiasApi
        [HttpGet]
        public async Task<ActionResult<PaginacionRespuesta<MetodologiaAtaque>>> GetMetodologias(
            [FromQuery] string? buscarTexto, // Para la barra de búsqueda en React Native
            [FromQuery] int pagina = 1,      // Página por defecto
            [FromQuery] int limite = 10)     // Cantidad de tarjetas por recarga
        {
            // 1. Iniciamos la consulta (aún no viaja a la base de datos)
            var query = _context.MetodologiaAtaque.AsQueryable();

            // 2. Aplicamos el Filtro de búsqueda (si el usuario escribió algo)
            if (!string.IsNullOrWhiteSpace(buscarTexto))
            {
                // EF Core traduce esto a un "ILIKE" o "LIKE" en PostgreSQL
                query = query.Where(m => m.Nombre.ToLower().Contains(buscarTexto.ToLower()));
            }

            // 3. Contamos los totales (necesario para la UI de React Native)
            var totalRegistros = await query.CountAsync();
            var totalPaginas = (int)Math.Ceiling(totalRegistros / (double)limite);

            // 4. Aplicamos el Paginado y ejecutamos la consulta final
            var metodologias = await query
                .Skip((pagina - 1) * limite) // Saltamos los registros de páginas anteriores
                .Take(limite)                // Tomamos solo la cantidad solicitada
                .ToListAsync();              // AQUÍ viaja la consulta a Neon

            // 5. Armamos la caja de respuesta
            var respuesta = new PaginacionRespuesta<MetodologiaAtaque>
            {
                Datos = metodologias,
                PaginaActual = pagina,
                TotalPaginas = totalPaginas,
                TotalRegistros = totalRegistros
            };

            return Ok(respuesta);
        }
    }

    public class PaginacionRespuesta<T>
    {
        public List<T> Datos { get; set; } = new List<T>();
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
        public int TotalRegistros { get; set; }
        public bool TieneSiguientePagina => PaginaActual < TotalPaginas;
    }
}