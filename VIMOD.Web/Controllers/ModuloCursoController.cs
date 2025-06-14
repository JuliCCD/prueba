using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VIMOD.Domain.Models;
using VIMOD.Infrastructure.Context;

namespace VIMOD.Web.Controllers
{
    public class ModuloCursoController : Controller
    {
        private readonly VIMODDbContext _context;

        public ModuloCursoController(VIMODDbContext context)
        {
            _context = context;
        }

        // GET: ModuloCurso
        public async Task<IActionResult> Index()
        {
            var modulos = _context.ModulosCurso.Include(m => m.Curso);
            return View(await modulos.ToListAsync());
        }

        // GET: ModuloCurso/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var moduloCurso = await _context.ModulosCurso
                .Include(m => m.Curso)
                .FirstOrDefaultAsync(m => m.IdModulo == id);

            if (moduloCurso == null)
                return NotFound();

            return View(moduloCurso);
        }

        // GET: ModuloCurso/Create
        public IActionResult Create()
        {
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "IdCurso", "Nombre");
            return View();
        }

        // POST: ModuloCurso/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Titulo,Descripcion,Orden,IdCurso")] ModuloCurso moduloCurso) // Quitar IdModulo
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(moduloCurso);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", $"Error al guardar: {ex.InnerException?.Message ?? ex.Message}");
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage); // O usa un logger
                }
            }

            // Recargar datos necesarios para la vista si hay error
            ViewBag.IdCurso = new SelectList(_context.Cursos, "IdCurso", "Nombre", moduloCurso?.IdCurso);
            return View(moduloCurso);
        }

        // GET: ModuloCurso/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var moduloCurso = await _context.ModulosCurso.FindAsync(id);
            if (moduloCurso == null)
                return NotFound();

            ViewData["IdCurso"] = new SelectList(_context.Cursos, "IdCurso", "Nombre", moduloCurso.IdCurso);
            return View(moduloCurso);
        }

        // POST: ModuloCurso/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdModulo,Titulo,Descripcion,Orden,IdCurso")] ModuloCurso moduloCurso)
        {
            if (id != moduloCurso.IdModulo)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(moduloCurso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuloCursoExists(moduloCurso.IdModulo))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "IdCurso", "Nombre", moduloCurso.IdCurso);
            return View(moduloCurso);
        }

        // GET: ModuloCurso/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var moduloCurso = await _context.ModulosCurso
                .Include(m => m.Curso)
                .FirstOrDefaultAsync(m => m.IdModulo == id);

            if (moduloCurso == null)
                return NotFound();

            return View(moduloCurso);
        }

        // POST: ModuloCurso/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var moduloCurso = await _context.ModulosCurso.FindAsync(id);
            _context.ModulosCurso.Remove(moduloCurso);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModuloCursoExists(int id)
        {
            return _context.ModulosCurso.Any(e => e.IdModulo == id);
        }
    }
}
