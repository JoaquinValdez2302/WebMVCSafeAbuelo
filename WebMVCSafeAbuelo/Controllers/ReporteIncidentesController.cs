
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMVCSafeAbuelo.Models;
using Microsoft.AspNetCore.Authorization;
using WebMVCSafeAbuelo.Services; // Aseguramos el acceso a la capa de servicios

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
        public async Task<IActionResult> Index()
        {
            return View(await _incidenteService.ObtenerTodosAsync());
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: REPORTEINCIDENTES/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FechaReporte,Localidad,PlataformaDeContacto,EjercePresionPsicologica,GeneraSentidoDeUrgencia,DescripcionDelEngaño,Estado")] ReporteIncidente reporteincidente)
        {
            if (ModelState.IsValid)
            {
                await _incidenteService.CrearAsync(reporteincidente);
                return RedirectToAction("Create", "EvidenciaIncidentes", new { reporteId = reporteincidente.Id });
            }
            return View(reporteincidente);
        }

        // GET: REPORTEINCIDENTES/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var reporteincidente = await _incidenteService.ObtenerPorIdAsync(id);

            if (reporteincidente == null) return NotFound();

            return View(reporteincidente);
        }

        // POST: REPORTEINCIDENTES/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,FechaReporte,Localidad,PlataformaDeContacto,EjercePresionPsicologica,GeneraSentidoDeUrgencia,DescripcionDelEngaño,Estado")] ReporteIncidente reporteincidente)
        {
            if (id != reporteincidente.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var reporteincidente = await _incidenteService.ObtenerPorIdAsync(id);

            if (reporteincidente == null) return NotFound();

            return View(reporteincidente);
        }

        // POST: REPORTEINCIDENTES/Delete/5
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