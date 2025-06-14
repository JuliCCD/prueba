using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIMOD.PruebasUnitarias
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using VIMOD.Domain.Models;
    using VIMOD.Infrastructure.Context;
    using VIMOD.Web.Controllers;
    using Microsoft.AspNetCore.Mvc;

    namespace PruebasFinal
    {
        [TestClass]
        public sealed class TiendaControllerTests
        {
            [TestMethod]
            public void CursosDisponibles_DeberiaRetornarVistaConListaDeCursos()
            {
            }
        }
    }
}
