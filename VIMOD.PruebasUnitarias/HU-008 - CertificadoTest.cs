using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using VIMOD.Domain.Models;
using VIMOD.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using VIMOD.Infrastructure.Context;

namespace VIMOD.PruebasUnitarias
{
    [TestClass]
    public sealed class CertificadoTest_H008
    {
        [TestMethod]
        public async Task ValidarCertificado()
        {
            //Preparar

            var certificado = new Certificado
            {
                CodigoUnico = "156816518"
            };

            //Accion
            certificado.CodigoUnico = "156816518";
            //Verificar

            Assert.AreEqual("156816518", certificado.CodigoUnico);
        }
    }
}
