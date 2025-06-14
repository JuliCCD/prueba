using System.Threading.Tasks;

namespace VIMOD.Repositories.Interfaces
{
    public interface IUnitWorkRepositorio
    {
        ICategoriaRepositorio Categoria { get; }
        ICertificadoRepositorio Certificado { get; }
        ICursoRepositorio Curso { get; }
        IUsuarioRepositorio Usuarios { get; }
        Task Guardar();
    }
}
