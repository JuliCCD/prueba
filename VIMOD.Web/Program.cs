using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using VIMOD.Domain.Models;
using VIMOD.Infrastructure.Context;
using VIMOD.Infrastructure.SeddData;
using VIMOD.Repositories.Implementaciones;
using VIMOD.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Configuraci�n de la conexi�n a la base de datos
var conexion = builder.Configuration.GetConnectionString("ConnectionSQLServer");
builder.Services.AddDbContext<VIMODDbContext>(options => options.UseSqlServer(conexion));

//Registramos el UnitWorkRepositorio
builder.Services.AddScoped<IUnitWorkRepositorio, UnitWorkRepositorio>();  // Aqu� se registra el servicio

// Indicar que est� trabajando con roles
builder.Services.AddIdentity<Usuario, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<VIMODDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// Llamada a los datos semillas para inicializar roles y usuario administrador
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await SeedData.InitializeAsync(services);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al inicializar los datos semillas: {ex.Message}");
    }
}

app.Run();
public partial class Program { } // <- Agrega esto al final del archivo
