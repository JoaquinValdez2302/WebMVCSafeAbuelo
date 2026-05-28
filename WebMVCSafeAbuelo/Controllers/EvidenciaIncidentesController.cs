
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebMVCSafeAbuelo.Data;
using WebMVCSafeAbuelo.Models;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class EvidenciaIncidentesController : Controller
{
    private readonly ApplicationDbContext _context;

    public EvidenciaIncidentesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: EVIDENCIAINCIDENTES
    public async Task<IActionResult> Index()    
    {
        return View(await _context.EvidenciasIncidentes.ToListAsync());
    }

    // GET: EVIDENCIAINCIDENTES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var evidenciaincidente = await _context.EvidenciasIncidentes
            .FirstOrDefaultAsync(m => m.Id == id);
        if (evidenciaincidente == null)
        {
            return NotFound();
        }

        return View(evidenciaincidente);
    }

    // GET: EVIDENCIAINCIDENTES/Create
    public IActionResult Create(int? reporteId)
    {
        // Si venimos redirigidos desde un reporte nuevo, preseleccionamos ese reporte
        if (reporteId.HasValue)
        {
            ViewData["ReporteIncidenteId"] = new SelectList(_context.ReportesIncidentes, "Id", "Id", reporteId.Value);
        }
        else
        {
            // Comportamiento normal si entramos directo a la página
            ViewData["ReporteIncidenteId"] = new SelectList(_context.ReportesIncidentes, "Id", "Id");
        }

        return View();
    }

    // POST: EVIDENCIAINCIDENTES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Tipo,Valor,Notas,ReporteIncidenteId,ReporteIncidente")] EvidenciaIncidente evidenciaincidente)
    {
        if (ModelState.IsValid)
        {
            _context.Add(evidenciaincidente);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "ReporteIncidentes", new { id = evidenciaincidente.ReporteIncidenteId });
        }
        return View(evidenciaincidente);
    }

    // GET: EVIDENCIAINCIDENTES/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var evidenciaincidente = await _context.EvidenciasIncidentes.FindAsync(id);
        if (evidenciaincidente == null)
        {
            return NotFound();
        }
        return View(evidenciaincidente);
    }

    // POST: EVIDENCIAINCIDENTES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Tipo,Valor,Notas,ReporteIncidenteId,ReporteIncidente")] EvidenciaIncidente evidenciaincidente)
    {
        if (id != evidenciaincidente.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(evidenciaincidente);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EvidenciaIncidenteExists(evidenciaincidente.Id))
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
        return View(evidenciaincidente);
    }

    // GET: EVIDENCIAINCIDENTES/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var evidenciaincidente = await _context.EvidenciasIncidentes
            .FirstOrDefaultAsync(m => m.Id == id);
        if (evidenciaincidente == null)
        {
            return NotFound();
        }

        return View(evidenciaincidente);
    }

    // POST: EVIDENCIAINCIDENTES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var evidenciaincidente = await _context.EvidenciasIncidentes.FindAsync(id);
        if (evidenciaincidente != null)
        {
            _context.EvidenciasIncidentes.Remove(evidenciaincidente);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool EvidenciaIncidenteExists(int? id)
    {
        return _context.EvidenciasIncidentes.Any(e => e.Id == id);
    }
}
