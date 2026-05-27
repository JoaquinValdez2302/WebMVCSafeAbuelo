
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMVCSafeAbuelo.Models;
using WebMVCSafeAbuelo.Data;

public class MetodologiaAtaquesController : Controller
{
    private readonly ApplicationDbContext _context;

    public MetodologiaAtaquesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: METODOLOGIAATAQUES
    public async Task<IActionResult> Index()    
    {
        return View(await _context.MetodologiaAtaque.ToListAsync());
    }

    // GET: METODOLOGIAATAQUES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var metodologiaataque = await _context.MetodologiaAtaque
            .FirstOrDefaultAsync(m => m.Id == id);
        if (metodologiaataque == null)
        {
            return NotFound();
        }

        return View(metodologiaataque);
    }

    // GET: METODOLOGIAATAQUES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: METODOLOGIAATAQUES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,PrincipalMotorPsicologico,SeñalesDeAlarma,AccionPreventiva,EstaActivo")] MetodologiaAtaque metodologiaataque)
    {
        if (ModelState.IsValid)
        {
            _context.Add(metodologiaataque);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(metodologiaataque);
    }

    // GET: METODOLOGIAATAQUES/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var metodologiaataque = await _context.MetodologiaAtaque.FindAsync(id);
        if (metodologiaataque == null)
        {
            return NotFound();
        }
        return View(metodologiaataque);
    }

    // POST: METODOLOGIAATAQUES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Nombre,Descripcion,PrincipalMotorPsicologico,SeñalesDeAlarma,AccionPreventiva,EstaActivo")] MetodologiaAtaque metodologiaataque)
    {
        if (id != metodologiaataque.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(metodologiaataque);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MetodologiaAtaqueExists(metodologiaataque.Id))
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
        return View(metodologiaataque);
    }

    // GET: METODOLOGIAATAQUES/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var metodologiaataque = await _context.MetodologiaAtaque
            .FirstOrDefaultAsync(m => m.Id == id);
        if (metodologiaataque == null)
        {
            return NotFound();
        }

        return View(metodologiaataque);
    }

    // POST: METODOLOGIAATAQUES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var metodologiaataque = await _context.MetodologiaAtaque.FindAsync(id);
        if (metodologiaataque != null)
        {
            _context.MetodologiaAtaque.Remove(metodologiaataque);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MetodologiaAtaqueExists(int? id)
    {
        return _context.MetodologiaAtaque.Any(e => e.Id == id);
    }
}
