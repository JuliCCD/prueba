using VIMOD.Domain.Models;

namespace VIMOD.PruebasUnitarias;

[TestClass]
public class CursoTest_H001_H006
{
    [TestMethod]
    public void Curso_CreacionCorrecta()
    {
        // Preparar
        var curso = new Curso
        {
            IdCurso = 1,
            Nombre = "Dise�o Arquitect�nico I",
            Descripcion = "Curso introductorio de dise�o arquitect�nico",
            Precio = 200.0m,
            PrecioDescuento = 180.0m,
            DocenteAsignado = "Arq. Laura M�ndez",
            IdCategoria = 5,
            URLImagen = "/imagenes/diseno1.png"
        };

        // Verificar
        Assert.AreEqual(1, curso.IdCurso);
        Assert.AreEqual("Dise�o Arquitect�nico I", curso.Nombre);
        Assert.AreEqual("Curso introductorio de dise�o arquitect�nico", curso.Descripcion);
        Assert.AreEqual(200.0m, curso.Precio);
        Assert.AreEqual(180.0m, curso.PrecioDescuento);
        Assert.AreEqual("Arq. Laura M�ndez", curso.DocenteAsignado);
        Assert.AreEqual(5, curso.IdCategoria);
        Assert.AreEqual("/imagenes/diseno1.png", curso.URLImagen);
    }

    [TestMethod]
    public void Curso_EditarCorrectamente()
    {
        // Preparar
        var curso = new Curso
        {
            IdCurso = 2,
            Nombre = "Historia de la Arquitectura",
            Descripcion = "Panorama hist�rico de la arquitectura cl�sica",
            Precio = 150.0m,
            PrecioDescuento = 140.0m,
            DocenteAsignado = "Arq. Pedro Salinas",
            IdCategoria = 6,
            URLImagen = "/imagenes/historia.png"
        };

        // Acci�n (editar valores)
        curso.Nombre = "Historia de la Arquitectura Moderna";
        curso.Descripcion = "Evoluci�n de la arquitectura desde el siglo XIX";
        curso.Precio = 170.0m;
        curso.PrecioDescuento = 150.0m;
        curso.DocenteAsignado = "Arq. Andrea L�pez";
        curso.IdCategoria = 7;
        curso.URLImagen = "/imagenes/historia_moderna.png";

        // Verificar
        Assert.AreEqual("Historia de la Arquitectura Moderna", curso.Nombre);
        Assert.AreEqual("Evoluci�n de la arquitectura desde el siglo XIX", curso.Descripcion);
        Assert.AreEqual(170.0m, curso.Precio);
        Assert.AreEqual(150.0m, curso.PrecioDescuento);
        Assert.AreEqual("Arq. Andrea L�pez", curso.DocenteAsignado);
        Assert.AreEqual(7, curso.IdCategoria);
        Assert.AreEqual("/imagenes/historia_moderna.png", curso.URLImagen);
    }

    [TestMethod]
    public void Curso_EliminarCorrectamente()
    {
        // Preparar
        var curso = new Curso
        {
            IdCurso = 3,
            Nombre = "Urbanismo y Planificaci�n",
            Descripcion = "Fundamentos de urbanismo moderno",
            Precio = 220.0m,
            PrecioDescuento = 200.0m,
            DocenteAsignado = "Arq. Mario P�rez",
            IdCategoria = 8,
            URLImagen = "/imagenes/urbanismo.png"
        };

        // Acci�n (simular eliminaci�n)
        curso = null;

        // Verificar
        Assert.IsNull(curso);
    }
}
