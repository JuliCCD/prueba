using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using VIMOD.Domain.Models; // Tu modelo Usuario

namespace VIMOD.PruebasUnitarias.Identity
{
    [TestClass]
    public class HU012_ActualizarPerfil
    {
        private Mock<UserManager<Usuario>> CrearUserManagerMock()
        {
            var store = new Mock<IUserStore<Usuario>>();
            return new Mock<UserManager<Usuario>>(
                store.Object, null, null, null, null, null, null, null, null);
        }

        [TestMethod]
        public async Task ActualizarTelefono_UsuarioValido_Exito()
        {
            // Arrange
            var userManagerMock = CrearUserManagerMock();
            var usuario = new Usuario
            {
                UserName = "prueba@correo.com",
                PhoneNumber = "888888888"
            };

            string nuevoTelefono = "999999999";

            userManagerMock.Setup(u => u.SetPhoneNumberAsync(usuario, nuevoTelefono))
                           .ReturnsAsync(IdentityResult.Success);

            // Act
            var resultado = await userManagerMock.Object.SetPhoneNumberAsync(usuario, nuevoTelefono);

            // Assert
            Assert.IsTrue(resultado.Succeeded, "El número de teléfono no se actualizó correctamente.");
        }
    }
}
