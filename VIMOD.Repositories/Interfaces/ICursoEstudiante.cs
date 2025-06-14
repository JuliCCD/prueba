using VIMOD.Domain.Models;

namespace VIMOD.Repositories.Interfaces
{
    public interface ICursoEstudiante : IRepositorioBase<Curso>
    {
        void Actualizar(Curso curso);
        Task<IEnumerable<Curso>> ObtenerByIdUserAsync(string id);
    }
}
