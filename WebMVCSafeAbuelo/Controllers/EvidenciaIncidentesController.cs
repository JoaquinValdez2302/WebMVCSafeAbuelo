
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;
using WebMVCSafeAbuelo.Data;
using WebMVCSafeAbuelo.Models;

[Authorize]
public class EvidenciaIncidentesController : Controller
{
    private readonly ApplicationDbContext _context;

    public EvidenciaIncidentesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: EVIDENCIAINCIDENTES
    // Hacemos que el método reciba el ID del reporte
    public async Task<IActionResult> Index(int? reporteId)
    {
        if (reporteId == null)
        {
            // Si alguien intenta entrar sin un reporte específico, lo devolvemos a la lista de incidentes
            return RedirectToAction("Index", "ReporteIncidentes");
        }

        // Guardamos el ID en el ViewBag para que la vista HTML sepa de qué reporte hablamos
        ViewBag.ReporteId = reporteId;

        // Filtramos la base de datos para traer solo las evidencias de este reporte
        var evidencias = await _context.EvidenciasIncidentes
            .Include(e => e.ReporteIncidente) 
            .Where(e => e.ReporteIncidenteId == reporteId)
            .ToListAsync();

        return View(evidencias);
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
    public IActionResult Create(int reporteId)
    {
        // Le pasamos el ID del reporte a la vista para que lo ponga en un campo oculto
        ViewBag.ReporteId = reporteId;
        return View();
    }

    // POST: EVIDENCIAINCIDENTES/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Tipo,Valor,ReporteIncidenteId")] EvidenciaIncidente evidenciaIncidente)
    {
        ModelState.Remove("ReporteIncidente");
        if (ModelState.IsValid)
        {
            // Guardamos usando exactamente el nombre de la variable que entró por el parámetro
            _context.Add(evidenciaIncidente);
            await _context.SaveChangesAsync();

            // Redirigimos usando el ID de ese mismo objeto
            return RedirectToAction(nameof(Index), new { reporteId = evidenciaIncidente.ReporteIncidenteId });
        }
        var listaDeErrores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

        // Los unimos en un solo texto gigante
        string mensajeDebug = "MODO DEPURACIÓN - Fallos detectados: " + string.Join(" | ", listaDeErrores);

        // Inyectamos el texto en el resumen de validación para que aparezca en rojo en la pantalla
        ModelState.AddModelError(string.Empty, mensajeDebug);
        // ------------------------------------------

        // Recargamos la vista
        ViewBag.ReporteId = evidenciaIncidente.ReporteIncidenteId;
        return View(evidenciaIncidente);
      
    }

    // GET: EVIDENCIAINCIDENTES/Edit/5
    [Authorize(Roles = "Administrador")]
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
    [Authorize(Roles = "Administrador")]
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
    [Authorize(Roles = "Administrador")]
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
    [Authorize(Roles = "Administrador")]
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
