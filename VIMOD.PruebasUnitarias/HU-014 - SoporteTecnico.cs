using Microsoft.VisualStudio.TestTools.UnitTesting;
using VIMOD.Web;

namespace VIMOD.PruebasUnitarias
{
    [TestClass]
    public class ContactoSoporte_H014
    {
        [TestMethod]
        public void ValidarFormulario_DeberiaRetornarTrue_SiTodoEsCorrecto()
        {
            // Arrange
            var modelo = new ContactoViewModel
            {
                Nombre = "Sebastián",
                Correo = "sebastian@ejemplo.com",
                Mensaje = "¡Hola soporte!"
            };
            var service = new ContactoService();

            // Act
            var esValido = service.ValidarFormulario(modelo);

            // Assert
            Assert.IsTrue(esValido);
        }

        [TestMethod]
        public void ValidarFormulario_DeberiaRetornarFalse_SiCorreoEsInvalido()
        {
            var modelo = new ContactoViewModel
            {
                Nombre = "Sebastián",
                Correo = "sebas.com",
                Mensaje = "Mensaje"
            };
            var service = new ContactoService();
            var esValido = service.ValidarFormulario(modelo);

            Assert.IsFalse(esValido);
        }

        [TestMethod]
        public void ValidarFormulario_DeberiaRetornarFalse_SiNombreVacio()
        {
            var modelo = new ContactoViewModel
            {
                Nombre = "",
                Correo = "correo@ejemplo.com",
                Mensaje = "Mensaje"
            };
            var service = new ContactoService();
            var esValido = service.ValidarFormulario(modelo);

            Assert.IsFalse(esValido);
        }
    }
}

public class ContactoViewModel
{
    public string Nombre { get; set; }
    public string Correo { get; set; }
    public string Mensaje { get; set; }
}

public class ContactoService
{
    public bool ValidarFormulario(ContactoViewModel modelo)
    {
        if (string.IsNullOrWhiteSpace(modelo.Nombre)) return false;
        if (string.IsNullOrWhiteSpace(modelo.Correo)) return false;
        if (string.IsNullOrWhiteSpace(modelo.Mensaje)) return false;
        if (!modelo.Correo.Contains("@")) return false;
        return true;
    }
}