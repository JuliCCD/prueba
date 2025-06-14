using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIMOD.Domain.Models
{
    public class ContenidoModulo
    {
        [Key]
        public int IdContenido { get; set; }

        [Required]
        public string Titulo { get; set; }

        [Required]
        public string? Tipo { get; set; } // Ej: "Video", "PDF", "Cuestionario"
        
        [Required]
        public string? URLContenido { get; set; } // Ruta del recurso

        public int Orden { get; set; }

        public int IdModulo { get; set; }
        
        [ForeignKey("IdModulo")]
        public ModuloCurso? ModuloCurso { get; set; }
    }
}
