using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIMOD.Domain.Models;
using VIMOD.Infrastructure.Context;

namespace VIMOD.Infrastructure.SeddData
{
    public class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
             
        {

            Console.WriteLine("🚀 INICIANDO DATOS SEMILLA...");
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Usuario>>();
            var context = serviceProvider.GetRequiredService<VIMODDbContext>();

            string[] roleNames = { "Administrador", "Estudiante" };

            // Crear roles si no existen
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            string adminEmail = "admin@vimod.com";
            string adminPassword = "Benja123_";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var user = new Usuario
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Nombre = "Benjamin",
                    Apellido = "RG",
                    Direccion = "Lima",
                    FechaDeNacimiento = DateTime.Now,
                    Telefono = "999999999"
                };

                var result = await userManager.CreateAsync(user, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Administrador");
                }
            }
        }
    }
}
