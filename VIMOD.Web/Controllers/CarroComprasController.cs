using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VIMOD.Domain.Models;
using VIMOD.Infrastructure.Context;

namespace VIMOD.Web.Controllers
{
    public class CarroComprasController : Controller
    {
        private readonly VIMODDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public CarroComprasController(VIMODDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: CarroCompras
        public async Task<IActionResult> Index()
        {
            var vIMODDbContext = _context.CarritoCompras.Include(c => c.Curso).Include(c => c.Usuario);
            return View(await vIMODDbContext.ToListAsync());
        }

        // GET: CarroCompras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carroCompras = await _context.CarritoCompras
                .Include(c => c.Curso)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.IdCarroCompras == id);
            if (carroCompras == null)
            {
                return NotFound();
            }

            return View(carroCompras);
        }

        // GET: CarroCompras/Create
        public IActionResult Create()
        {
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "IdCurso", "Nombre");
            ViewData["UsuarioId"] = new SelectList(_context.Set<Usuario>(), "Id", "Id");
            return View();
        }

        // POST: CarroCompras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCarroCompras,Cantidad,UsuarioId,IdCurso")] CarroCompras carroCompras)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carroCompras);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "IdCurso", "Nombre", carroCompras.IdCurso);
            ViewData["UsuarioId"] = new SelectList(_context.Set<Usuario>(), "Id", "Id", carroCompras.UsuarioId);
            return View(carroCompras);
        }

        // GET: CarroCompras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carroCompras = await _context.CarritoCompras.FindAsync(id);
            if (carroCompras == null)
            {
                return NotFound();
            }
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "IdCurso", "Nombre", carroCompras.IdCurso);
            ViewData["UsuarioId"] = new SelectList(_context.Set<Usuario>(), "Id", "Id", carroCompras.UsuarioId);
            return View(carroCompras);
        }

        // POST: CarroCompras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCarroCompras,Cantidad,UsuarioId,IdCurso")] CarroCompras carroCompras)
        {
            if (id != carroCompras.IdCarroCompras)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carroCompras);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarroComprasExists(carroCompras.IdCarroCompras))
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
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "IdCurso", "Nombre", carroCompras.IdCurso);
            ViewData["UsuarioId"] = new SelectList(_context.Set<Usuario>(), "Id", "Id", carroCompras.UsuarioId);
            return View(carroCompras);
        }

        // GET: CarroCompras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carroCompras = await _context.CarritoCompras
                .Include(c => c.Curso)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.IdCarroCompras == id);
            if (carroCompras == null)
            {
                return NotFound();
            }

            return View(carroCompras);
        }

        // POST: CarroCompras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carroCompras = await _context.CarritoCompras.FindAsync(id);
            if (carroCompras != null)
            {
                _context.CarritoCompras.Remove(carroCompras);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // POST: CarroCompras/Comprar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Comprar()
        {
            var usuarioId = _userManager.GetUserId(User);

            var carrito = await _context.CarritoCompras
                            .Include(c => c.Curso)
                            .Where(c => c.UsuarioId == usuarioId)
                            .ToListAsync();

            if (!carrito.Any())
            {
                TempData["Error"] = "No tienes cursos en el carrito.";
                return RedirectToAction("Index");
            }

            var pedido = new Pedido
            {
                FechaPedido = DateTime.Now,
                FormaPago = "Tarjeta de crédito", // Simulación por ahora
                EstadoPedido = "Pagado",
                TotalPedido = carrito.Sum(c => c.Curso.PrecioDescuento > 0 ? c.Curso.PrecioDescuento : c.Curso.Precio),
                UserId = usuarioId,
                Direccion = "No requerida",
                CodigoPostal = "0000",
                Provincia = "N/A",
                Localidad = "N/A",
                Telefono = "N/A",
                Detalles = new List<PedidoDetalle>()
            };

            foreach (var item in carrito)
            {
                pedido.Detalles.Add(new PedidoDetalle
                {
                    Cantidad = item.Cantidad,
                    PrecioIndividual = item.Curso.PrecioDescuento > 0 ? item.Curso.PrecioDescuento : item.Curso.Precio,
                    IdCurso = item.IdCurso
                });
            }

            _context.Pedidos.Add(pedido);
            _context.CarritoCompras.RemoveRange(carrito);

            await _context.SaveChangesAsync();

            TempData["Satisfactorio"] = "Compra realizada con éxito.";
            return RedirectToAction("MisCursos", "Curso");
        }


        private bool CarroComprasExists(int id)
        {
            return _context.CarritoCompras.Any(e => e.IdCarroCompras == id);
        }
    }
}
