using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIMOD.Domain.Models
{
    public class ProgresoEstudiante
    {
        [Key]
        public int IdProgreso { get; set; }

        public string IdUsuario { get; set; } // O el tipo que uses para el usuario
        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; }

        public int IdContenido { get; set; }
        [ForeignKey("IdContenido")]
        public ContenidoModulo ContenidoModulo { get; set; }

        public bool Completado { get; set; }

        public DateTime FechaCompletado { get; set; }
    }
}
