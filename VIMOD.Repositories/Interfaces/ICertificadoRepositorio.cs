using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIMOD.Domain.Models;

namespace VIMOD.Repositories.Interfaces
{
    public interface ICertificadoRepositorio : IRepositorioBase<Certificado>
    {
        void Actualizar(Certificado certificado);
    }
}
