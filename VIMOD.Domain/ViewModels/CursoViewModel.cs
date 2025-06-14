using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIMOD.Domain.ViewModels
{
    public class CursoViewModel
    {
        public int IdCurso { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public decimal? Precio { get; set; }
        public decimal? PrecioDescuento { get; set; }

        public string DocenteAsignado { get; set; }

        public int IdCategoria { get; set; }

        public IFormFile ImagenArchivo { get; set; } // este es para cargar archivo
    }
}
