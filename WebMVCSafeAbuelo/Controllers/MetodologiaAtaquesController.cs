
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebMVCSafeAbuelo.Models;
using WebMVCSafeAbuelo.Services;

[Authorize]
public class MetodologiaAtaquesController : Controller
{
    private readonly IMetodologiaService _metodologiaService;

    public MetodologiaAtaquesController(IMetodologiaService metodologiaService)
    {
        _metodologiaService = metodologiaService;
    }

    // GET: METODOLOGIAATAQUES
    public async Task<IActionResult> Index(string searchString, int? pageNumber)
    {
        int pageSize = 10;
        int pagina = pageNumber ?? 0;

        // Delegamos la paginación y búsqueda al servicio
        var resultado = await _metodologiaService.ObtenerPaginadosAsync(searchString, pagina + 1, pageSize);

        ViewBag.SearchString = searchString;
        ViewBag.PageNumber = pagina;
        ViewBag.TotalPaginas = resultado.TotalPaginas;

        return View(resultado.Metodologias);
    }

    // GET: METODOLOGIAATAQUES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var metodologia = await _metodologiaService.ObtenerPorIdAsync(id);
        if (metodologia == null) return NotFound();

        return View(metodologia);
    }

    // GET: METODOLOGIAATAQUES/Create
    [Authorize(Roles = "Administrador")]
    public IActionResult Create() => View();

    // POST: METODOLOGIAATAQUES/Create
    [Authorize(Roles = "Administrador")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,PrincipalMotorPsicologico,SeñalesDeAlarma,AccionPreventiva,EstaActivo")] MetodologiaAtaque metodologia)
    {
        if (ModelState.IsValid)
        {
            await _metodologiaService.CrearAsync(metodologia);
            return RedirectToAction(nameof(Index));
        }
        return View(metodologia);
    }

    // GET: METODOLOGIAATAQUES/Edit/5
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var metodologia = await _metodologiaService.ObtenerPorIdAsync(id);
        if (metodologia == null) return NotFound();

        return View(metodologia);
    }

    // POST: METODOLOGIAATAQUES/Edit/5
    [Authorize(Roles = "Administrador")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,PrincipalMotorPsicologico,SeñalesDeAlarma,AccionPreventiva,EstaActivo")] MetodologiaAtaque metodologia)
    {
        if (id != metodologia.Id) return NotFound();

        if (ModelState.IsValid)
        {
            await _metodologiaService.ActualizarAsync(metodologia);
            return RedirectToAction(nameof(Index));
        }
        return View(metodologia);
    }

    // GET: METODOLOGIAATAQUES/Delete/5
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var metodologia = await _metodologiaService.ObtenerPorIdAsync(id);
        if (metodologia == null) return NotFound();

        return View(metodologia);
    }

    // POST: METODOLOGIAATAQUES/Delete/5
    [Authorize(Roles = "Administrador")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _metodologiaService.EliminarAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
