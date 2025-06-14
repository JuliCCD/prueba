using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VIMOD.Domain.Models;

namespace VIMOD.Tests
{
    [TestClass]
    public class UsuarioTests
    {
        [TestMethod]
        public void HU_020_RegistroUsuario_ModeloValido_DeberiaSerValido()
        {
            // Arrange
            var usuario = new Usuario
            {
                Nombre = "Carlos",
                Apellido = "Lopez",
                Direccion = "carlos.lopez@correo.com",
                PhoneNumber = "999888777",
                FechaDeNacimiento = new DateTime(1990, 12, 1)
            };

            // Act
            var resultados = new List<ValidationResult>();
            var contexto = new ValidationContext(usuario, null, null);
            bool esValido = Validator.TryValidateObject(usuario, contexto, resultados, true);

            // Assert
            Assert.IsTrue(esValido);
            Assert.AreEqual(0, resultados.Count);
        }

        [TestMethod]
        public void HU_020_RegistroUsuario_ModeloInvalido_DeberiaRetornarErrores()
        {
            // Arrange: Falta Nombre y correo mal escrito
            var usuario = new Usuario
            {
                Apellido = "Gómez",
                Direccion = "correo-invalido",
                FechaDeNacimiento = DateTime.Now
            };

            // Act
            var resultados = new List<ValidationResult>();
            var contexto = new ValidationContext(usuario, null, null);
            bool esValido = Validator.TryValidateObject(usuario, contexto, resultados, true);

            // Assert
            Assert.IsFalse(esValido);
            Assert.IsTrue(resultados.Count >= 1);
        }
    }
}
