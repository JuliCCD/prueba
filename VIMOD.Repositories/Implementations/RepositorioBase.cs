using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VIMOD.Infrastructure.Context;
using VIMOD.Repositories.Interfaces;

namespace VIMOD.Repositories.Implementaciones
{
    public class RepositorioBase<T> : IRepositorioBase<T> where T : class
    {
        private readonly VIMODDbContext _context;
        internal DbSet<T> dbSet;

        public RepositorioBase(VIMODDbContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }

        public async Task AgregarAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public void Eliminar(T entity)
        {
            dbSet.Remove(entity);
            _context.SaveChangesAsync();
        }

        public void EliminarRango(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
            _context.SaveChangesAsync();
        }

        public async Task<T> ObtenerByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<T> ObtenerPrimeroAsync(Expression<Func<T, bool>> filtro = null, string incluirPropiedades = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (filtro is not null)
                query = query.Where(filtro); // select * from where

            if (incluirPropiedades is not null)
                foreach (var incluirPropiedad in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(incluirPropiedad); // "Category,Cliente"

            if (!isTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> ObtenerTodosAsync(Expression<Func<T, bool>> filtro = null, Func<IQueryable<T>, IOrderedQueryable<T>> ordenarPor = null, string incluirPropiedades = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (filtro is not null)
                query = query.Where(filtro); // select * from where

            if (incluirPropiedades is not null)
                foreach (var incluirPropiedad in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(incluirPropiedad); // "Category,Cliente"

            if (ordenarPor is not null)
                query = ordenarPor(query);

            if (isTracking)
                query = query.AsNoTracking();

            return await query.ToListAsync();
        }
    }
}
