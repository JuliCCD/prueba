using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VIMOD.Domain.Models;
using VIMOD.Infrastructure.Context;
using VIMOD.Web.Controllers;

namespace VIMOD.PruebasUnitarias;

[TestClass]
public class ContenidoEducativoTests_H002
{
    [TestClass]
    public class ContenidoEducativoControllerTests
    {
        [TestMethod]
        public async Task Create_DeberiaRegistrarContenidoEducativoCorrectamente()
        {
            // Arrange: Configura el contexto en memoria
            var options = new DbContextOptionsBuilder<VIMODDbContext>()
                .UseInMemoryDatabase(databaseName: "ContenidoEducativoTestDb")
                .Options;

            using (var context = new VIMODDbContext(options))
            {
                // Agrega un curso para la relación FK
                context.Cursos.Add(new Curso
                {
                    IdCurso = 1,
                    Nombre = "Curso Prueba",
                    Descripcion = "Descripción del curso",
                    Precio = 0
                });
                context.SaveChanges();

                var controller = new ContenidoEducativoController(context);

                // Simula el contenido a registrar
                var contenido = new ContenidoEducativo
                {
                    Titulo = "Contenido de Prueba",
                    Descripcion = "Contenido de prueba unitaria",
                    UrlContenido = "https://example.com/contenido1",
                    CursoId = 1,
                    FechaPublicacion = DateTime.Now,
                    EsActivo = true
                };

                // Act: Ejecuta el método Create POST
                var result = await controller.Create(contenido) as RedirectToActionResult;

                // Assert: Verifica que se haya redirigido correctamente y el contenido esté en la base
                Assert.IsNotNull(result);
                Assert.AreEqual("Index", result.ActionName);

                var contenidoEnDb = context.ContenidosEducativos.FirstOrDefault(c => c.Titulo == "Contenido de Prueba");
                Assert.IsNotNull(contenidoEnDb);
                Assert.AreEqual("Contenido de prueba unitaria", contenidoEnDb.Descripcion);
                Assert.AreEqual("https://example.com/contenido1", contenidoEnDb.UrlContenido);
                Assert.IsTrue(contenidoEnDb.EsActivo);
            }
        }
    }
}
