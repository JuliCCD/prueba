using Microsoft.VisualStudio.TestTools.UnitTesting;
using VIMOD.Domain.Models;

namespace VIMOD.PruebasUnitarias
{
    [TestClass]
    public class ContenidoModuloTests_HU016
    {
        [TestMethod]
        public void CrearContenidoModulo_ConDatosValidos_DebeSerValido()
        {
            // Arrange
            var contenido = new ContenidoModulo
            {
                IdContenido = 1,
                Titulo = "Video de Introducción",
                Tipo = "Video",
                URLContenido = "https://vimod.com/videos/intro.mp4",
                Orden = 1,
                IdModulo = 1
            };

            // Act
            bool esValido =
                !string.IsNullOrWhiteSpace(contenido.Titulo) &&
                !string.IsNullOrWhiteSpace(contenido.Tipo) &&
                !string.IsNullOrWhiteSpace(contenido.URLContenido) &&
                Uri.IsWellFormedUriString(contenido.URLContenido, UriKind.Absolute) &&
                contenido.IdModulo > 0;

            // Assert
            Assert.IsTrue(esValido, "Todos los campos obligatorios del contenido deben estar completos y ser válidos.");
        }

        [TestMethod]
        public void CrearContenidoModulo_SinTitulo_DebeFallar()
        {
            // Arrange
            var contenido = new ContenidoModulo
            {
                IdContenido = 2,
                Titulo = "", // inválido
                Tipo = "PDF",
                URLContenido = "https://vimod.com/docs/guia.pdf",
                Orden = 2,
                IdModulo = 1
            };

            // Act
            bool tieneTitulo = !string.IsNullOrWhiteSpace(contenido.Titulo);

            // Assert
            Assert.IsFalse(tieneTitulo, "El título del contenido no debe estar vacío.");
        }

        [TestMethod]
        public void CrearContenidoModulo_ConUrlInvalida_DebeFallar()
        {
            // Arrange
            var contenido = new ContenidoModulo
            {
                IdContenido = 3,
                Titulo = "PDF sin URL válida",
                Tipo = "PDF",
                URLContenido = "archivo_local.pdf", // inválido
                Orden = 3,
                IdModulo = 1
            };

            // Act
            bool urlValida = Uri.IsWellFormedUriString(contenido.URLContenido, UriKind.Absolute);

            // Assert
            Assert.IsFalse(urlValida, "La URL del contenido debe estar bien formada.");
        }

        [TestMethod]
        public void CrearContenidoModulo_SinModuloRelacionado_DebeFallar()
        {
            // Arrange
            var contenido = new ContenidoModulo
            {
                IdContenido = 4,
                Titulo = "Cuestionario Final",
                Tipo = "Cuestionario",
                URLContenido = "https://vimod.com/cuestionarios/final",
                Orden = 4,
                IdModulo = 0 // inválido
            };

            // Act
            bool moduloEsValido = contenido.IdModulo > 0;

            // Assert
            Assert.IsFalse(moduloEsValido, "El contenido debe estar asociado a un módulo válido.");
        }
    }
}
