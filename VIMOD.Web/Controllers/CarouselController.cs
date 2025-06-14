using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VIMOD.Domain.Models;
using VIMOD.Infrastructure.Context;
using Microsoft.AspNetCore.Http;

namespace VIMOD.Web.Controllers
{
    public class CarouselController : Controller
    {
        private readonly VIMODDbContext _context;
        private const string ImageFolder = "img/carousel"; // Carpeta relativa en wwwroot

        public CarouselController(VIMODDbContext context)
        {
            _context = context;
        }

        // GET: Carousel
        public async Task<IActionResult> Index()
        {
            var carouseles = await _context.Carouseles
                           .Where(c => c.Activo)
                           .OrderBy(c => c.Orden)
                           .ToListAsync();

            return View(carouseles);
        }

        // GET: Carousel/Create
        public IActionResult Create()
        {
            return View(new Carousel { Orden = _context.Carouseles.Count() + 1 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Carousel carousel, IFormFile imagen)
        {
            if (imagen == null || imagen.Length == 0)
            {
                ModelState.AddModelError("ImagenUrl", "Debe seleccionar una imagen.");
                return View(carousel);
            }

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", ImageFolder);
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imagen.FileName)}";
            var fullPath = Path.Combine(uploadFolder, fileName);
            var relativePath = $"/{ImageFolder}/{fileName}";

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await imagen.CopyToAsync(stream);
            }

            carousel.ImagenUrl = relativePath;

            _context.Add(carousel);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Carousel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var carousel = await _context.Carouseles.FindAsync(id);
            if (carousel == null)
                return NotFound();

            return View(carousel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descripcion,Orden,Activo,ImagenUrl")] Carousel carousel, IFormFile nuevaImagen)
        {
            if (id != carousel.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var registroExistente = await _context.Carouseles.FindAsync(id);

                    if (registroExistente == null)
                        return NotFound();

                    // Actualiza los campos
                    registroExistente.Titulo = carousel.Titulo;
                    registroExistente.Descripcion = carousel.Descripcion;
                    registroExistente.Orden = carousel.Orden;
                    registroExistente.Activo = carousel.Activo;

                    if (nuevaImagen != null && nuevaImagen.Length > 0)
                    {
                        var nombreArchivo = Path.GetFileName(nuevaImagen.FileName);
                        var rutaGuardado = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagenes", nombreArchivo);

                        using (var stream = new FileStream(rutaGuardado, FileMode.Create))
                        {
                            await nuevaImagen.CopyToAsync(stream);
                        }

                        // Actualiza la ruta de la imagen
                        registroExistente.ImagenUrl = "/imagenes/" + nombreArchivo;
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error actualizando imagen: " + ex.Message);
                }
            }
            return View(carousel);
        }

        // GET: Carousel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var carousel = await _context.Carouseles.FirstOrDefaultAsync(m => m.Id == id);
            if (carousel == null) return NotFound();

            return View(carousel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carousel = await _context.Carouseles.FindAsync(id);
            if (carousel != null)
            {
                if (!string.IsNullOrEmpty(carousel.ImagenUrl))
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", carousel.ImagenUrl.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.Carouseles.Remove(carousel);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CarouselExists(int id)
        {
            return _context.Carouseles.Any(e => e.Id == id);
        }
    }
}
