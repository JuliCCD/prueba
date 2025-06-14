using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIMOD.Domain.Models
{
    public class ModuloCurso
    {
        [Key]
        public int IdModulo { get; set; }

        [Required]
        public string Titulo { get; set; }

        public string? Descripcion { get; set; }

        public int Orden { get; set; } // Para mantener el orden secuencial

        public int IdCurso { get; set; }
        
        [ForeignKey("IdCurso")]
        public Curso? Curso { get; set; }

        public virtual ICollection<ContenidoModulo>? Contenidos { get; set; } // <-- Nullable y sin [Required]
    }
}
