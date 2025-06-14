using VIMOD.Infrastructure.Context;
using VIMOD.Repositories.Implementaciones;
using VIMOD.Repositories.Interfaces;

namespace VIMOD.Repositories.Implementaciones
{
    public class UnitWorkRepositorio : IUnitWorkRepositorio
    {
        private readonly VIMODDbContext _context;

        public ICategoriaRepositorio Categoria { get; private set; }
        public ICertificadoRepositorio Certificado {  get; private set; }
        public ICursoRepositorio Curso {  get; private set; }
        public IUsuarioRepositorio Usuarios {  get; private set; }

        public UnitWorkRepositorio(VIMODDbContext context)
        {
            _context = context;
            Categoria = new CategoriaRepositorio(context);
            Usuarios = new UsuarioRepositorio(context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task Guardar()
        {
            await _context.SaveChangesAsync();
        }
    }
}
