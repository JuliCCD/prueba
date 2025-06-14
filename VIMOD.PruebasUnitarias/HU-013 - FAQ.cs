using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Threading.Tasks;

namespace VIMOD.PruebasUnitarias
{
    [TestClass]
    public class HU_013_FAQ
    {
        private readonly WebApplicationFactory<Program> _factory;

        public HU_013_FAQ()
        {
            _factory = new WebApplicationFactory<Program>();
        }

        [TestMethod]
        public async Task CargarVistaFAQ_DeberiaRetornarStatus200()
        {
            // Arranca 
            var client = _factory.CreateClient();

            // Actua
            var response = await client.GetAsync("/Home/FAQ");

            // Ejecuta
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "La vista FAQ no retornó un código 200 OK.");
        }
    }
}
