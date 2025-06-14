using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VIMOD.Domain.Models;
using VIMOD.Infrastructure.Context;

namespace VIMOD.Web.Controllers
{
    public class ContenidoEducativoController : Controller
    {
        private readonly VIMODDbContext _context;

        public ContenidoEducativoController(VIMODDbContext context)
        {
            _context = context;
        }

        // ✅ Acceso público al listado
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var contenidos = _context.ContenidosEducativos.Include(c => c.Curso);
            return View(await contenidos.ToListAsync());
        }

        // ✅ Acceso público a los detalles
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var contenido = await _context.ContenidosEducativos
                .Include(c => c.Curso)
                .FirstOrDefaultAsync(m => m.ContenidoId == id);

            if (contenido == null)
                return NotFound();

            return View(contenido);
        }

        // 🔐 Solo Administradores pueden crear
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            ViewData["CursoId"] = new SelectList(_context.Cursos, "IdCurso", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([Bind("ContenidoId,Titulo,Descripcion,UrlContenido,CursoId,FechaPublicacion,EsActivo")] ContenidoEducativo contenidoEducativo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contenidoEducativo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CursoId"] = new SelectList(_context.Cursos, "IdCurso", "Nombre", contenidoEducativo.CursoId);
            return View(contenidoEducativo);
        }

        // 🔐 Solo Administradores pueden editar
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var contenido = await _context.ContenidosEducativos.FindAsync(id);
            if (contenido == null)
                return NotFound();

            ViewData["CursoId"] = new SelectList(_context.Cursos, "IdCurso", "Nombre", contenido.CursoId);
            return View(contenido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id, [Bind("ContenidoId,Titulo,Descripcion,UrlContenido,CursoId,FechaPublicacion,EsActivo")] ContenidoEducativo contenidoEducativo)
        {
            if (id != contenidoEducativo.ContenidoId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contenidoEducativo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContenidoEducativoExists(contenidoEducativo.ContenidoId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CursoId"] = new SelectList(_context.Cursos, "IdCurso", "Nombre", contenidoEducativo.CursoId);
            return View(contenidoEducativo);
        }

        // 🔐 Solo Administradores pueden eliminar
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var contenido = await _context.ContenidosEducativos
                .Include(c => c.Curso)
                .FirstOrDefaultAsync(m => m.ContenidoId == id);

            if (contenido == null)
                return NotFound();

            return View(contenido);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contenido = await _context.ContenidosEducativos.FindAsync(id);
            if (contenido != null)
                _context.ContenidosEducativos.Remove(contenido);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContenidoEducativoExists(int id)
        {
            return _context.ContenidosEducativos.Any(e => e.ContenidoId == id);
        }
    }
}
