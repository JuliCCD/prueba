using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using VIMOD.Domain.Models;
using VIMOD.Infrastructure.Context;

public class ContenidoModuloController : Controller
{
    private readonly VIMODDbContext _context;

    public ContenidoModuloController(VIMODDbContext context)
    {
        _context = context;
    }

    // GET: ContenidoModulo
    public async Task<IActionResult> Index()
    {
        var contenidos = _context.ContenidosModulo
            .Include(c => c.ModuloCurso)
            .OrderBy(c => c.Orden); // Se puede ajustar el orden si se desea
        return View(await contenidos.ToListAsync());
    }

    // GET: ContenidoModulo/Create
    public IActionResult Create()
    {
        ViewData["IdModulo"] = new SelectList(_context.ModulosCurso, "IdModulo", "Titulo");
        return View(new ContenidoModulo());
    }

    // POST: ContenidoModulo/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdContenido,Titulo,Tipo,URLContenido,Orden,IdModulo")] ContenidoModulo contenidoModulo)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _context.Add(contenidoModulo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(string.Empty, "Error al guardar los datos. Detalles: " + ex.Message);
            }
        }

        // Mostrar errores si la validación falla
        foreach (var state in ModelState)
        {
            foreach (var error in state.Value.Errors)
            {
                Console.WriteLine($"Campo: {state.Key}, Error: {error.ErrorMessage}");
            }
        }

        ViewData["IdModulo"] = new SelectList(_context.ModulosCurso, "IdModulo", "Titulo", contenidoModulo.IdModulo);
        return View(contenidoModulo);
    }

    // GET: ContenidoModulo/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var contenido = await _context.ContenidosModulo.FindAsync(id);
        if (contenido == null)
            return NotFound();

        ViewData["IdModulo"] = new SelectList(_context.ModulosCurso, "IdModulo", "Titulo", contenido.IdModulo);
        return View(contenido);
    }

    // POST: ContenidoModulo/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdContenido,Titulo,Tipo,URLContenido,Orden,IdModulo")] ContenidoModulo contenidoModulo)
    {
        if (id != contenidoModulo.IdContenido)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(contenidoModulo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContenidoModuloExists(contenidoModulo.IdContenido))
                    return NotFound();
                else
                    throw;
            }
        }

        ViewData["IdModulo"] = new SelectList(_context.ModulosCurso, "IdModulo", "Titulo", contenidoModulo.IdModulo);
        return View(contenidoModulo);
    }

    // GET: ContenidoModulo/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var contenido = await _context.ContenidosModulo
            .Include(c => c.ModuloCurso)
            .FirstOrDefaultAsync(m => m.IdContenido == id);

        if (contenido == null)
            return NotFound();

        return View(contenido);
    }
    // GET: ContenidoModulo/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var contenido = await _context.ContenidosModulo
            .Include(c => c.ModuloCurso)
            .FirstOrDefaultAsync(m => m.IdContenido == id);

        if (contenido == null)
        {
            return NotFound();
        }

        return View(contenido);
    }

    // POST: ContenidoModulo/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var contenido = await _context.ContenidosModulo.FindAsync(id);
        if (contenido != null)
        {
            _context.ContenidosModulo.Remove(contenido);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool ContenidoModuloExists(int id)
    {
        return _context.ContenidosModulo.Any(e => e.IdContenido == id);
    }
}
