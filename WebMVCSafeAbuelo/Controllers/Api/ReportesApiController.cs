using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebMVCSafeAbuelo.DTOs;
using WebMVCSafeAbuelo.Services;

namespace WebMVCSafeAbuelo.Controllers.Api
{
    [Route("api/reporte")] // Cumpliendo el contrato exacto
    [ApiController]
    public class ReportesApiController : ControllerBase
    {
        private readonly IIncidenteService _incidenteService;

        public ReportesApiController(IIncidenteService incidenteService)
        {
            _incidenteService = incidenteService;
        }

        // GET /api/reporte/?page=<page-number>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReporteDetalleDto>>> GetReportes([FromQuery] int page = 1)
        {
            int pageSize = 10; // Cantidad de reportes por página
            var reportes = await _incidenteService.ObtenerReportesAceptadosPaginadosAsync(page, pageSize);
            return Ok(reportes);
        }

        // GET /api/reporte/<id>
        [HttpGet("{id}")]
        public async Task<ActionResult<ReporteDetalleDto>> GetReporteById(int id)
        {
            var reporte = await _incidenteService.ObtenerReportePorIdAsync(id);

            if (reporte == null)
            {
                return NotFound(new { mensaje = "Reporte no encontrado." });
            }

            return Ok(reporte);
        }

        // GET /api/reporte/user/<user-id>
        [HttpGet("user/{userId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // Exige que la petición venga con un Token JWT válido
        public async Task<ActionResult<IEnumerable<ReporteDetalleDto>>> GetReportesByUser(string userId)
        {
            // Medida defensiva: Extraemos el ID del usuario directamente del Token JWT de Firebase
            
            var tokenUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("user_id")?.Value;

            // VERIFICACIÓN CRÍTICA DEL CONTRATO: 
            // Para que un usuario que no es el dueño no pueda acceder a los datos
            if (tokenUserId != userId)
            {
                return StatusCode(403, new { mensaje = "Acceso denegado. El token no coincide con el usuario solicitado." });
            }

            var reportes = await _incidenteService.ObtenerReportesPorUsuarioAsync(userId);
            return Ok(reportes);
        }

        // POST /api/reporte
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // Solo usuarios autenticados pueden enviar reportes
        public async Task<ActionResult<ReporteDetalleDto>> CreateReporte([FromBody] ReporteAltaDto dto)
        {
            // Validamos que el cuerpo de la petición cumpla con el DTO (Required, etc.)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validamos que el usuario del token sea el mismo que el AuthorId que intentan enviar
            var tokenUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("user_id")?.Value;
            if (tokenUserId != dto.AuthorId)
            {
                return StatusCode(403, new { mensaje = "No puedes crear un reporte en nombre de otro usuario." });
            }

            try
            {
                var reporteCreado = await _incidenteService.CrearReporteDesdeApiAsync(dto);
                
                // Devuelve 201 Created y la URL para consultar el nuevo recurso
                return CreatedAtAction(nameof(GetReporteById), new { id = reporteCreado.Id }, reporteCreado);
            }
            catch (Exception ex)
            {
                // En producción, aquí deberías loguear el 'ex.Message' internamente
                return StatusCode(500, new { mensaje = "Error interno al procesar los datos enviados. Revisa los valores de los Enums." });
            }
        }
    }
}