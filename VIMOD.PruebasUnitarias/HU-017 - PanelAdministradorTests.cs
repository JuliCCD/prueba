using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using VIMOD.Domain.Models;
using VIMOD.Infrastructure.Context;

namespace VIMOD.PruebasUnitarias
{
    [TestClass]
    public class HU017_PanelAdministradorTests
    {
        private DbContextOptions<VIMODDbContext> _options;

        [TestInitialize]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<VIMODDbContext>()
                .UseInMemoryDatabase(databaseName: "VIMOD_Test_DB_HU017")
                .Options;

            using (var context = new VIMODDbContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                // Crear contenido relacionado con módulo
                var contenido = new ContenidoModulo
                {
                    IdContenido = 1,
                    IdModulo = 1,
                    Titulo = "Video introductorio",
                    Tipo = "Video",
                    URLContenido = "https://vimod.com/video.mp4",
                    Orden = 1
                };

                // Crear módulo relacionado con curso
                var modulo = new ModuloCurso
                {
                    IdModulo = 1,
                    IdCurso = 1,
                    Titulo = "Módulo 1",
                    Orden = 1,
                    Contenidos = new List<ContenidoModulo> { contenido }
                };

                // Crear curso con módulo
                var curso = new Curso
                {
                    IdCurso = 1,
                    Nombre = "Curso de Programación",
                    Descripcion = "Aprende C# desde cero",
                    Precio = 150.0m,
                    PrecioDescuento = 120.0m,
                    DocenteAsignado = "Ing. Sofía Rivera",
                    IdCategoria = 3,
                    URLImagen = "/imagenes/programacion.png",
                    Modulos = new List<ModuloCurso> { modulo }
                };

                context.Cursos.Add(curso);
                context.SaveChanges();
            }
        }

        [TestMethod]
        public void HU017_EliminarCurso_ConRelaciones_ValidarEliminacionManual()
        {
            using (var context = new VIMODDbContext(_options))
            {
                // Captura 1: Estado inicial de la base de datos (antes de eliminar)
                // ➤ Evidencia visual: mostrar que el curso, módulos y contenidos existen.

                var curso = context.Cursos
                    .Include(c => c.Modulos)
                        .ThenInclude(m => m.Contenidos)
                    .FirstOrDefault(c => c.IdCurso == 1);

                Assert.IsNotNull(curso, "Curso no encontrado.");
                Assert.AreEqual(1, curso.Modulos.Count, "No se encontró el módulo.");
                Assert.AreEqual(1, curso.Modulos.First().Contenidos.Count, "No se encontró el contenido del módulo.");

                // Eliminar contenidos manualmente
                foreach (var modulo in curso.Modulos)
                {
                    context.ContenidosModulo.RemoveRange(modulo.Contenidos);
                }

                // Eliminar módulos manualmente
                context.ModulosCurso.RemoveRange(curso.Modulos);

                // Eliminar curso
                context.Cursos.Remove(curso);
                context.SaveChanges();
            }

            using (var context = new VIMODDbContext(_options))
            {
                // Captura 2: Verificar que no existen registros
                // ➤ Evidencia visual: mostrar que ya no hay registros en las tablas implicadas

                var cursoEliminado = context.Cursos.FirstOrDefault(c => c.IdCurso == 1);
                var modulosRestantes = context.ModulosCurso.Where(m => m.IdCurso == 1).ToList();
                var contenidosRestantes = context.ContenidosModulo.Where(c => c.IdModulo == 1).ToList();

                // Verificación de eliminación
                Assert.IsNull(cursoEliminado, "El curso no fue eliminado.");
                Assert.AreEqual(0, modulosRestantes.Count, "Los módulos no fueron eliminados.");
                Assert.AreEqual(0, contenidosRestantes.Count, "Los contenidos no fueron eliminados.");
            }
        }
    }
}
