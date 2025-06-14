using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VIMOD.Domain.Models;

namespace VIMOD.Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdminController : Controller
    {
        private readonly UserManager<Usuario> _userManager;

        public AdminController(UserManager<Usuario> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult ListaUsuarios()
        {
            var usuarios = _userManager.Users.ToList();
            return View(usuarios);
        }

        [HttpGet]
        public async Task<IActionResult> EditarUsuario(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }
        [HttpPost]
        public async Task<IActionResult> EditarUsuario(Usuario model)
        {
            var usuario = await _userManager.FindByIdAsync(model.Id);
            if (usuario == null) return NotFound();

            // Actualizar solo los campos editables
            usuario.UserName = model.UserName;
            usuario.Email = model.Email;
            usuario.PhoneNumber = model.PhoneNumber;
            usuario.Nombre = model.Nombre;
            usuario.Apellido = model.Apellido;
            usuario.Direccion = model.Direccion;
            usuario.FechaDeNacimiento = model.FechaDeNacimiento;

            var result = await _userManager.UpdateAsync(usuario);
            if (result.Succeeded)
                return RedirectToAction("ListaUsuarios");

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EliminarUsuario(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null) return NotFound();

            if (await _userManager.IsInRoleAsync(usuario, "Administrador"))
            {
                ModelState.AddModelError("", "No se puede eliminar al administrador.");
                return RedirectToAction("ListaUsuarios");
            }

            // Eliminar manualmente los roles asignados
            var roles = await _userManager.GetRolesAsync(usuario);
            if (roles.Any())
            {
                var removeRolesResult = await _userManager.RemoveFromRolesAsync(usuario, roles);
                if (!removeRolesResult.Succeeded)
                {
                    ModelState.AddModelError("", "Error al quitar roles del usuario.");
                    return RedirectToAction("ListaUsuarios");
                }
            }

            var result = await _userManager.DeleteAsync(usuario);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Error al eliminar el usuario.");
            }

            return RedirectToAction("ListaUsuarios");
        }

    }
}
