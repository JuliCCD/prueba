using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIMOD.Domain.Models;

namespace VIMOD.Repositories.Interfaces
{
    public interface IUsuarioRepositorio : IRepositorioBase<Usuario>
    {
        void Actualizar(Usuario usuario);
    }
}
