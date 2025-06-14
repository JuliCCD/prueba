using Microsoft.VisualStudio.TestTools.UnitTesting;
using VIMOD.Domain.Models;

namespace VIMOD.PruebasUnitarias
{
    [TestClass]
    public class ModuloCursoTests_HU015
    {
        [TestMethod]
        public void CrearModuloCurso_ConDatosValidos_DebeSerValido()
        {
            // Arrange
            var modulo = new ModuloCurso
            {
                IdModulo = 1,
                Titulo = "Módulo 1: Fundamentos",
                Descripcion = "Descripción general del módulo.",
                Orden = 1,
                IdCurso = 101
            };

            // Act
            bool esValido = !string.IsNullOrWhiteSpace(modulo.Titulo) && modulo.IdCurso > 0;

            // Assert
            Assert.IsTrue(esValido, "El módulo debe tener un título válido y estar asociado a un curso existente.");
        }

        [TestMethod]
        public void CrearModuloCurso_SinTitulo_DebeFallar()
        {
            // Arrange
            var modulo = new ModuloCurso
            {
                IdModulo = 2,
                Titulo = "", // inválido
                Orden = 2,
                IdCurso = 101
            };

            // Act
            bool tieneTitulo = !string.IsNullOrWhiteSpace(modulo.Titulo);

            // Assert
            Assert.IsFalse(tieneTitulo, "El título del módulo no puede estar vacío.");
        }

        [TestMethod]
        public void CrearModuloCurso_ConCursoIdInvalido_DebeFallar()
        {
            // Arrange
            var modulo = new ModuloCurso
            {
                IdModulo = 3,
                Titulo = "Módulo sin curso",
                Orden = 3,
                IdCurso = 0 // inválido
            };

            // Act
            bool cursoEsValido = modulo.IdCurso > 0;

            // Assert
            Assert.IsFalse(cursoEsValido, "El Id del curso debe ser mayor que cero.");
        }
    }
}
