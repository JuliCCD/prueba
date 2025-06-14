using VIMOD.Domain.Models;
using VIMOD.Infrastructure.Context;
using VIMOD.Repositories.Interfaces;

namespace VIMOD.Repositories.Implementaciones
{
    public class UsuarioRepositorio : RepositorioBase<Usuario>, IUsuarioRepositorio
    {
        private readonly VIMODDbContext _context;
        public UsuarioRepositorio(VIMODDbContext context) : base(context)
        {
            _context = context;
        }

        public void Actualizar(Usuario inscripcion)
        {
            /*var categoryDB = _context.Category.FirstOrDefault(c => c.CategoryId == category.CategoryId);
            if (categoryDB is not null)
            {
                categoryDB.Name = category.Name;
                categoryDB.Status = category.Status;
                categoryDB.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
            }*/
        }
    }
}
