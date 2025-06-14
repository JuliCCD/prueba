using VIMOD.Domain.Models;
using VIMOD.Infrastructure.Context;
using VIMOD.Repositories.Interfaces;
namespace VIMOD.Repositories.Implementaciones
{
    public class CategoriaRepositorio : RepositorioBase<Categoria>, ICategoriaRepositorio
    {
        private readonly VIMODDbContext _context;
        
        public CategoriaRepositorio(VIMODDbContext context) : base(context)
        {
            _context = context;
        }

        public void Actualiza(Categoria categoria)
        {
            var categoryDB = _context.Categorias.FirstOrDefault(c => c.Id == categoria.Id);
            if (categoryDB is not null)
            {
                categoryDB.Nombre = categoria.Nombre;
                categoryDB.Estado = categoria.Estado;
                categoryDB.Descripcion = categoria.Descripcion;

                _context.SaveChanges();
            }
        }
    }
}
