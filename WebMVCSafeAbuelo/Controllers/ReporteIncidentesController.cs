
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WebMVCSafeAbuelo.Models;
using WebMVCSafeAbuelo.Services;

namespace WebMVCSafeAbuelo.Controllers
{
    [Authorize]
    public class ReporteIncidentesController : Controller
    {
        private readonly IIncidenteService _incidenteService;

        public ReporteIncidentesController(IIncidenteService incidenteService)
        {
            _incidenteService = incidenteService;
        }

        // GET: REPORTEINCIDENTES
        public async Task<IActionResult> Index(string buscar, int pagina = 1)
        { 
            int cantidadPorPagina = 5; // Mostrar 5 incidentes por pantalla

            // Llamamos a nuestro nuevo super-método
            var resultado = await _incidenteService.ObtenerPaginadosAsync(buscar, pagina, cantidadPorPagina);

            // Guardamos variables de estado en el ViewBag para que la vista HTML pueda dibujar los botones
            ViewBag.Buscar = buscar;
            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = resultado.TotalPaginas;

            return View(resultado.Incidentes);
        }

        // GET: REPORTEINCIDENTES/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var reporteincidente = await _incidenteService.ObtenerPorIdAsync(id);

            if (reporteincidente == null) return NotFound();

            return View(reporteincidente);
        }

        // GET: REPORTEINCIDENTES/Create
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: REPORTEINCIDENTES/Create
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Provincia,Localidad,FechaReporte,PlataformaDeContacto,PlataformaOtra,EjercePresionPsicologica,GeneraSentidoDeUrgencia,DescripcionDelEngaño,Estado")] ReporteIncidente reporte)
        {
            if (ModelState.IsValid)
            {
                reporte.FechaReporte = DateTime.SpecifyKind(reporte.FechaReporte, DateTimeKind.Utc);
                try
                {
                    // Le pasamos el trabajo pesado a la capa de servicio
                    // Nota: Cambia "CrearAsync" por el nombre exacto que tenga el método en tu interfaz IIncidenteService
                    await _incidenteService.CrearAsync(reporte);

                    return RedirectToAction("Index", "EvidenciaIncidentes", new { reporteId = reporte.Id }); 
                }
                catch (Exception ex)
                {
                    string errorReal = ex.Message;
                    string detalleInterno = ex.InnerException != null ? ex.InnerException.Message : "Sin detalle interno";

                    // Mostramos el error crudo en la pantalla para poder leerlo
                    ModelState.AddModelError(string.Empty, $"MODO DEPURACIÓN - Error: {errorReal} | Detalle: {detalleInterno}");
                }
            }

            return View(reporte);
        }

        // GET: REPORTEINCIDENTES/Edit/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var reporteincidente = await _incidenteService.ObtenerPorIdAsync(id);

            if (reporteincidente == null) return NotFound();

            return View(reporteincidente);
        }

        // POST: REPORTEINCIDENTES/Edit/5
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,AuthorId,Provincia,FechaReporte,Localidad,PlataformaDeContacto,PlataformaOtra,EjercePresionPsicologica,GeneraSentidoDeUrgencia,DescripcionDelEngaño,Estado")] ReporteIncidente reporteincidente)
        {
            if (id != reporteincidente.Id) return NotFound();
            ModelState.Remove("Autor");
            ModelState.Remove("Evidencias");
            if (ModelState.IsValid)
            {
                try
                {
                    reporteincidente.FechaReporte = DateTime.SpecifyKind(reporteincidente.FechaReporte, DateTimeKind.Utc);
                    await _incidenteService.ActualizarAsync(reporteincidente);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_incidenteService.Existe(reporteincidente.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(reporteincidente);
        }

        // GET: REPORTEINCIDENTES/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var reporteincidente = await _incidenteService.ObtenerPorIdAsync(id);

            if (reporteincidente == null) return NotFound();

            return View(reporteincidente);
        }

        // POST: REPORTEINCIDENTES/Delete/5
        [Authorize(Roles = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id != null)
            {
                await _incidenteService.EliminarAsync(id.Value);
            }
            return RedirectToAction(nameof(Index));
        }
        
    }

    

    } 
