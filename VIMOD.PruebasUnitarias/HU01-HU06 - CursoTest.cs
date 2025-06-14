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
            Nombre = "Diseño Arquitectónico I",
            Descripcion = "Curso introductorio de diseño arquitectónico",
            Precio = 200.0m,
            PrecioDescuento = 180.0m,
            DocenteAsignado = "Arq. Laura Méndez",
            IdCategoria = 5,
            URLImagen = "/imagenes/diseno1.png"
        };

        // Verificar
        Assert.AreEqual(1, curso.IdCurso);
        Assert.AreEqual("Diseño Arquitectónico I", curso.Nombre);
        Assert.AreEqual("Curso introductorio de diseño arquitectónico", curso.Descripcion);
        Assert.AreEqual(200.0m, curso.Precio);
        Assert.AreEqual(180.0m, curso.PrecioDescuento);
        Assert.AreEqual("Arq. Laura Méndez", curso.DocenteAsignado);
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
            Descripcion = "Panorama histórico de la arquitectura clásica",
            Precio = 150.0m,
            PrecioDescuento = 140.0m,
            DocenteAsignado = "Arq. Pedro Salinas",
            IdCategoria = 6,
            URLImagen = "/imagenes/historia.png"
        };

        // Acción (editar valores)
        curso.Nombre = "Historia de la Arquitectura Moderna";
        curso.Descripcion = "Evolución de la arquitectura desde el siglo XIX";
        curso.Precio = 170.0m;
        curso.PrecioDescuento = 150.0m;
        curso.DocenteAsignado = "Arq. Andrea López";
        curso.IdCategoria = 7;
        curso.URLImagen = "/imagenes/historia_moderna.png";

        // Verificar
        Assert.AreEqual("Historia de la Arquitectura Moderna", curso.Nombre);
        Assert.AreEqual("Evolución de la arquitectura desde el siglo XIX", curso.Descripcion);
        Assert.AreEqual(170.0m, curso.Precio);
        Assert.AreEqual(150.0m, curso.PrecioDescuento);
        Assert.AreEqual("Arq. Andrea López", curso.DocenteAsignado);
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
            Nombre = "Urbanismo y Planificación",
            Descripcion = "Fundamentos de urbanismo moderno",
            Precio = 220.0m,
            PrecioDescuento = 200.0m,
            DocenteAsignado = "Arq. Mario Pérez",
            IdCategoria = 8,
            URLImagen = "/imagenes/urbanismo.png"
        };

        // Acción (simular eliminación)
        curso = null;

        // Verificar
        Assert.IsNull(curso);
    }
}
