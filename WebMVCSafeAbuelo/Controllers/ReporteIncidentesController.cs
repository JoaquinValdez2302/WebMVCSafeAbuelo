
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMVCSafeAbuelo.Models;
using WebMVCSafeAbuelo.Data;
using Microsoft.AspNetCore.Authorization;


[Authorize]
public class ReporteIncidentesController : Controller
{
    private readonly ApplicationDbContext _context;

    public ReporteIncidentesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: REPORTEINCIDENTES
    public async Task<IActionResult> Index()    
    {
        return View(await _context.ReportesIncidentes.ToListAsync());
    }

    // GET: REPORTEINCIDENTES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var reporteincidente = await _context.ReportesIncidentes
            .FirstOrDefaultAsync(m => m.Id == id);
        if (reporteincidente == null)
        {
            return NotFound();
        }

        return View(reporteincidente);
    }

    // GET: REPORTEINCIDENTES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: REPORTEINCIDENTES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,FechaReporte,Localidad,PlataformaDeContacto,EjercePresionPsicologica,GeneraSentidoDeUrgencia,DescripcionDelEngaño,Estado")] ReporteIncidente reporteincidente)
    {
        if (ModelState.IsValid)
        {
            reporteincidente.FechaReporte = DateTime.SpecifyKind(reporteincidente.FechaReporte, DateTimeKind.Utc);
            _context.Add(reporteincidente);
            await _context.SaveChangesAsync();
            return RedirectToAction("Create", "EvidenciaIncidentes", new { reporteId = reporteincidente.Id });
        }
        return View(reporteincidente);      
    }

    // GET: REPORTEINCIDENTES/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var reporteincidente = await _context.ReportesIncidentes.FindAsync(id);
        if (reporteincidente == null)
        {
            return NotFound();
        }
        return View(reporteincidente);
    }

    // POST: REPORTEINCIDENTES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,FechaReporte,Localidad,PlataformaDeContacto,EjercePresionPsicologica,GeneraSentidoDeUrgencia,DescripcionDelEngaño,Estado")] ReporteIncidente reporteincidente)
    {
        if (id != reporteincidente.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(reporteincidente);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReporteIncidenteExists(reporteincidente.Id))
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
        if (id == null)
        {
            return NotFound();
        }

        var reporteincidente = await _context.ReportesIncidentes
            .FirstOrDefaultAsync(m => m.Id == id);
        if (reporteincidente == null)
        {
            return NotFound();
        }

        return View(reporteincidente);
    }

    // POST: REPORTEINCIDENTES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var reporteincidente = await _context.ReportesIncidentes.FindAsync(id);
        if (reporteincidente != null)
        {
            _context.ReportesIncidentes.Remove(reporteincidente);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ReporteIncidenteExists(int? id)
    {
        return _context.ReportesIncidentes.Any(e => e.Id == id);
    }
}
