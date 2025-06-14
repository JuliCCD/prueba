using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VIMOD.Domain.Models;
using VIMOD.Domain.ViewModels;
using VIMOD.Infrastructure.Context;
using VIMOD.Utilities;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;

namespace VIMOD.Web.Controllers
{
    public class CursoController : Controller
    {
        private readonly VIMODDbContext _context;
        private readonly IHostEnvironment _hostEnvironment;

        public CursoController(VIMODDbContext context, IHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Curso
        public async Task<IActionResult> Index()
        {
            var vIMODDbContext = _context.Cursos.Include(c => c.Categoria);
            return View(await vIMODDbContext.ToListAsync());
        }

        // GET: Curso/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var curso = await _context.Cursos
                .Include(c => c.Categoria)
                .Include(c => c.Modulos)
                    .ThenInclude(m => m.Contenidos)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.IdCurso == id);

            if (curso == null) return NotFound();

            return View(curso);
        }

        // GET: Curso/Create
        public IActionResult Create()
        {
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "Id", "Descripcion");
            return View();
        }

        // POST: Curso/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CursoViewModel model)
        {
            if (ModelState.IsValid)
            {
                string rutaImagen = null;

                if (model.ImagenArchivo != null && model.ImagenArchivo.Length > 0)
                {
                    string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(model.ImagenArchivo.FileName);
                    string rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "cursos");

                    if (!Directory.Exists(rutaCarpeta))
                    {
                        Directory.CreateDirectory(rutaCarpeta);
                    }

                    string rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);

                    using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        await model.ImagenArchivo.CopyToAsync(stream);
                    }

                    rutaImagen = "/img/cursos/" + nombreArchivo;
                }

                // Mapear manualmente el ViewModel a la entidad Curso
                var curso = new Curso
                {
                    Nombre = model.Nombre,
                    Descripcion = model.Descripcion,
                    Precio = model.Precio ?? 0, // si es null, asigna 0
                    PrecioDescuento = model.PrecioDescuento ?? 0,
                    DocenteAsignado = model.DocenteAsignado,
                    IdCategoria = model.IdCategoria,
                    URLImagen = rutaImagen // guardar la ruta de la imagen
                };


                _context.Add(curso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "Id", "Nombre", model.IdCategoria);
            return View(model);
        }


        // GET: Curso/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
            {
                return NotFound();
            }

            var viewModel = new CursoViewModel
            {
                IdCurso = curso.IdCurso,
                Nombre = curso.Nombre,
                Descripcion = curso.Descripcion,
                Precio = curso.Precio,
                PrecioDescuento = curso.PrecioDescuento,
                DocenteAsignado = curso.DocenteAsignado,
                IdCategoria = curso.IdCategoria,
                // Nota: ImagenArchivo no se asigna aquí porque se carga desde el formulario al momento del POST
            };

            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "Id", "Descripcion", curso.IdCategoria);
            ViewBag.ImagenActual = curso.URLImagen;
            return View(viewModel);

        }

        // POST: Curso/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CursoViewModel model)
        {
            if (id != model.IdCurso)
                return NotFound();

            if (ModelState.IsValid)
            {
                var curso = await _context.Cursos.FindAsync(id);
                if (curso == null)
                    return NotFound();

                try
                {
                    // Actualizar campos del curso
                    curso.Nombre = model.Nombre;
                    curso.Descripcion = model.Descripcion;
                    curso.Precio = model.Precio ?? curso.Precio;
                    curso.PrecioDescuento = model.PrecioDescuento ?? curso.PrecioDescuento;
                    curso.DocenteAsignado = model.DocenteAsignado;
                    curso.IdCategoria = model.IdCategoria;


                    // Si no se ha subido una nueva imagen, no modificar la URLImagen
                    if (model.ImagenArchivo != null && model.ImagenArchivo.Length > 0)
                    {
                        string extension = Path.GetExtension(model.ImagenArchivo.FileName).ToLowerInvariant();
                        var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                        if (!extensionesPermitidas.Contains(extension))
                        {
                            ModelState.AddModelError("ImagenArchivo", "El formato de la imagen no es válido. Solo se permiten archivos JPG, JPEG, PNG o GIF.");
                            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "Id", "Descripcion", model.IdCategoria);
                            return View(model);
                        }

                        // Eliminar la imagen anterior si existe
                        if (!string.IsNullOrEmpty(curso.URLImagen))
                        {
                            string rutaImagenAnterior = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", curso.URLImagen.TrimStart('/'));
                            if (System.IO.File.Exists(rutaImagenAnterior))
                                System.IO.File.Delete(rutaImagenAnterior);
                        }

                        // Guardar la nueva imagen
                        string nombreArchivo = Guid.NewGuid() + extension;
                        string rutaCarpeta = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "img", "cursos");

                        if (!Directory.Exists(rutaCarpeta))
                            Directory.CreateDirectory(rutaCarpeta);

                        string rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);

                        using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                        {
                            await model.ImagenArchivo.CopyToAsync(stream);
                        }

                        curso.URLImagen = "/img/cursos/" + nombreArchivo;
                    }

                    // No actualices URLImagen si no hay nueva imagen

                    _context.Update(curso);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al actualizar el curso: " + ex.Message);
                }
            }

            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "Id", "Descripcion", model.IdCategoria);
            return View(model);
        }


        // GET: Curso/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos
                .Include(c => c.Categoria)
                .FirstOrDefaultAsync(m => m.IdCurso == id);
            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }

        // POST: Curso/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso != null)
            {
                _context.Cursos.Remove(curso);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Curso/Buscar
        public async Task<IActionResult> Buscar(string nombre, string categoria, string docente)
        {
            var query = _context.Cursos.Include(c => c.Categoria).AsQueryable();

            if (!string.IsNullOrEmpty(nombre))
            {
                query = query.Where(c => c.Nombre.Contains(nombre));
            }

            if (!string.IsNullOrEmpty(categoria))
            {
                query = query.Where(c => c.Categoria.Nombre.Contains(categoria));
            }

            if (!string.IsNullOrEmpty(docente))
            {
                query = query.Where(c => c.DocenteAsignado.Contains(docente));
            }

            var resultados = await query.ToListAsync();
            return View(resultados);
        }


        private bool CursoExists(int id)
        {
            return _context.Cursos.Any(e => e.IdCurso == id);
        }
    }
}
