using Microsoft.AspNetCore.Mvc;
using VIMOD.Infrastructure.Context;
using VIMOD.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using VIMOD.Domain.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Security.Claims;

namespace VIMOD.Web.Controllers
{
    [Authorize]
    public class TiendaController : Controller
    {
        private readonly VIMODDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public TiendaController(VIMODDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Mostrar todos los cursos disponibles
        [AllowAnonymous]
        public IActionResult CursosDisponibles()
        {
            var cursos = _context.Cursos.ToList();
            return View(cursos);
        }

        [HttpPost]
        public IActionResult GuardarPedido(PedidoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("FinalizarPedido", model);
            }

            var nuevoPedido = new Pedido
            {
                FechaPedido = DateTime.Now,
                FormaPago = model.FormaPago,
                EstadoPedido = "Pendiente",
                TotalPedido = model.TotalPedido,
                Direccion = model.Direccion,
                CodigoPostal = model.CodigoPostal,
                Provincia = model.Provincia,
                Localidad = model.Localidad,
                Telefono = model.Telefono,
                UserId = model.UserId
            };

            _context.Pedidos.Add(nuevoPedido);
            _context.SaveChanges();

            int pedidoId = nuevoPedido.IdPedido;

            if (model.FormaPago == "Yape")
            {
                ViewBag.NombreCurso = model.NombreCurso;
                ViewBag.Monto = model.TotalPedido.ToString("F2");
                ViewBag.UrlQR = "https://www.chullitohost.com/assets/img/pagos/pago-min-01.png";

                return View("PagoYape");
            }

            return RedirectToAction("ConfirmacionTarjeta", new { id = pedidoId });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult FinalizarPedido(PedidoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.FormaPago == "Yape")
            {
                return RedirectToAction("PagarConYape", new { idCurso = model.IdCurso });
            }
            else if (model.FormaPago == "PayPal")
            {
                return RedirectToAction("PagarConPayPal", new { idCurso = model.IdCurso });
            }
            else
            {
                ModelState.AddModelError("", "Debe seleccionar un método de pago válido.");
                return View(model);
            }
        }

        [AllowAnonymous]
        public IActionResult PagarConYape(int idCurso)
        {
            var curso = _context.Cursos.FirstOrDefault(c => c.IdCurso == idCurso);
            if (curso == null)
                return NotFound();

            var numeroYape = "999999999";
            var monto = curso.Precio;
            var dataQR = $"00020101021126360014PE*5110001000000000520{numeroYape}53039865404{monto:0.00}5802PE6304";
            var urlQR = $"https://api.qrserver.com/v1/create-qr-code/?size=250x250&data={Uri.EscapeDataString(dataQR)}";

            ViewBag.NombreCurso = curso.Nombre;
            ViewBag.Monto = monto;
            ViewBag.UrlQR = urlQR;

            return View("PagoYape");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmarPagoYape(string nombreCurso)
        {
            ViewBag.Mensaje = $"Gracias por tu pago. El curso '{nombreCurso}' ha sido registrado como pagado.";
            return View("ConfirmacionPago");
        }

        [AllowAnonymous]
        public IActionResult PagarConPayPal(int idCurso)
        {
            var curso = _context.Cursos.FirstOrDefault(c => c.IdCurso == idCurso);
            if (curso == null)
                return NotFound();

            ViewBag.NombreCurso = curso.Nombre;
            ViewBag.Monto = curso.Precio;

            return View("PagoPayPal");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> FinalizarPedido(int idCurso, decimal precio, string metodo)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curso = _context.Cursos.FirstOrDefault(c => c.IdCurso == idCurso);
            if (curso == null)
                return NotFound();

            var model = new PedidoViewModel
            {
                IdCurso = curso.IdCurso,
                NombreCurso = curso.Nombre,
                TotalPedido = precio,
                FormaPago = metodo,
                UserId = userId
            };

            if (User.Identity.IsAuthenticated)
            {
                var usuario = await _userManager.GetUserAsync(User);
                if (usuario != null)
                {
                    model.NombreUsuario = usuario.Nombre + " " + usuario.Apellido;
                    model.Email = usuario.Email;
                    model.Direccion = usuario.Direccion;
                }
            }

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmarPagoPayPal(string nombreCurso)
        {
            ViewBag.Mensaje = $"Gracias por tu pago vía PayPal. El curso '{nombreCurso}' ha sido registrado como pagado.";
            return View("ConfirmacionPago");
        }

        [AllowAnonymous]
        public IActionResult Pagar(int id)
        {
            var curso = _context.Cursos.FirstOrDefault(c => c.IdCurso == id);
            if (curso == null)
                return NotFound();

            return View(curso);
        }
    }
}
