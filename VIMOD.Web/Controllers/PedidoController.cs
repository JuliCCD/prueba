using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VIMOD.Domain.Models;
using VIMOD.Domain.ViewModels;
using VIMOD.Infrastructure.Context;

namespace VIMOD.Web.Controllers
{
    public class PedidoController : Controller
    {
        private readonly VIMODDbContext _context;
        private readonly UserManager<Usuario> _userManager;


        public PedidoController(VIMODDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Pedido
        public async Task<IActionResult> Index()
        {
            var pedidos = _context.Pedidos.Include(p => p.IdUsuario);
            return View(await pedidos.ToListAsync());
        }

        // GET: Pedido/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var pedido = await _context.Pedidos
                .Include(p => p.IdUsuario)
                .FirstOrDefaultAsync(m => m.IdPedido == id);

            if (pedido == null) return NotFound();

            return View(pedido);
        }

        // GET: Pedido/Create
        public async Task<IActionResult> Create()
        {
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null) return Unauthorized();

            var pedido = new Pedido
            {
                Direccion = usuario.Direccion,
                Telefono = usuario.Telefono,
                UserId = usuario.Id,
                FechaPedido = DateTime.Now,
                EstadoPedido = "Pendiente"
            };

            return View(pedido);
        }

        // POST: Pedido/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FormaPago,TotalPedido,Direccion,CodigoPostal,Provincia,Localidad,Telefono")] Pedido pedido)
        {
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null) return Unauthorized();

            if (ModelState.IsValid)
            {
                pedido.UserId = usuario.Id;
                pedido.FechaPedido = DateTime.Now;
                pedido.EstadoPedido = "Pendiente";

                _context.Add(pedido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(pedido);
        }

        // GET: Pedido/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null) return NotFound();

            ViewData["UserId"] = new SelectList(_context.Set<Usuario>(), "Id", "Id", pedido.UserId);
            return View(pedido);
        }

        // POST: Pedido/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPedido,FechaPedido,FormaPago,EstadoPedido,TotalPedido,Direccion,CodigoPostal,Provincia,Localidad,Telefono,UserId")] Pedido pedido)
        {
            if (id != pedido.IdPedido) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedido.IdPedido)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["UserId"] = new SelectList(_context.Set<Usuario>(), "Id", "Id", pedido.UserId);
            return View(pedido);
        }

        // GET: Pedido/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var pedido = await _context.Pedidos
                .Include(p => p.IdUsuario)
                .FirstOrDefaultAsync(m => m.IdPedido == id);

            if (pedido == null) return NotFound();

            return View(pedido);
        }

        // POST: Pedido/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido != null)
            {
                _context.Pedidos.Remove(pedido);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedidos.Any(e => e.IdPedido == id);
        }

        public async Task<IActionResult> FinalizarPedido(int idCurso)
        {
            var curso = await _context.Cursos.FirstOrDefaultAsync(c => c.IdCurso == idCurso);
            if (curso == null) return NotFound();

            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null) return RedirectToAction("Login", "Account");

            var model = new PedidoViewModel
            {
                NombreUsuario = usuario.Nombre,
                Email = usuario.Email,
                IdCurso = curso.IdCurso,
                NombreCurso = curso.Nombre,
                TotalPedido = curso.Precio
            };

            return View(model); // <-- ESTO ES IMPORTANTE
        }

        [HttpPost]
        public IActionResult CambiarEstado(int id, string nuevoEstado)
        {
            var pedido = _context.Pedidos.Find(id);
            if (pedido == null)
            {
                return NotFound();
            }

            pedido.EstadoPedido = nuevoEstado;
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        public async Task<IActionResult> GuardarPedido(PedidoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("FinalizarPedido", model);
            }

            var usuario = await _userManager.GetUserAsync(User);

            if (usuario == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var pedido = new Pedido
            {
                FechaPedido = DateTime.Now,
                FormaPago = model.FormaPago,
                EstadoPedido = "Pendiente",
                TotalPedido = model.TotalPedido,
                Direccion = model.Direccion,
                CodigoPostal = model.CodigoPostal,
                Provincia = model.Provincia,
                Localidad = model.Localidad ?? "", // Aseguramos que no sea null
                Telefono = model.Telefono,
                UserId = usuario.Id // <-- usuario.Id es string (IdentityUser)
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return RedirectToAction("Confirmacion"); // o donde quieras llevar al usuario
        }

    }
}
