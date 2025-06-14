using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using VIMOD.Web.Controllers;
using VIMOD.Domain.ViewModels;
using VIMOD.Domain.Models;
using VIMOD.Infrastructure.Context;
using System.Linq;

[TestClass]
public class TiendaControllerTests
{
    private TiendaController _controller;
    private VIMODDbContext _context;

    [TestInitialize]
    public void Setup()
    {

        var options = new DbContextOptionsBuilder<VIMODDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        _context = new VIMODDbContext(options);


        _context.Pedidos.RemoveRange(_context.Pedidos);
        _context.SaveChanges();

        _controller = new TiendaController(_context, null);
    }

    [TestMethod]
    public void GuardarPedido_ModelValido_Yape_RetornaVistaPagoYape()
    {
        var model = new PedidoViewModel
        {
            FormaPago = "Yape",
            TotalPedido = 100,
            NombreCurso = "Curso Test",
            Direccion = "Direccion",
            CodigoPostal = "12345",
            Provincia = "Provincia",
            Localidad = "Localidad",
            Telefono = "999999999",
            UserId = "user1"
        };

        var result = _controller.GuardarPedido(model);

        Assert.IsInstanceOfType(result, typeof(ViewResult));
        var viewResult = result as ViewResult;

        Assert.AreEqual("PagoYape", viewResult.ViewName);
        Assert.AreEqual(model.NombreCurso, viewResult.ViewData["NombreCurso"]);
        Assert.AreEqual(model.TotalPedido.ToString("F2"), viewResult.ViewData["Monto"]);
        Assert.IsNotNull(viewResult.ViewData["UrlQR"]);

        Assert.AreEqual(1, _context.Pedidos.Count());
        Assert.AreEqual("Pendiente", _context.Pedidos.First().EstadoPedido);
    }


}
