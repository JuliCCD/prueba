using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIMOD.Domain.Models
{
    public class ContenidoEducativo
    {
        [Key]
        public int ContenidoId { get; set; }

        [Required]
        [StringLength(150)]
        public string Titulo { get; set; }

        [Required]
        [StringLength(300)]
        public string Descripcion { get; set; }

        [StringLength(500)]
        public string? UrlContenido { get; set; }  // Enlace al recurso (video, pdf, etc.)

        // Relación con Curso
        [Required]
        public int CursoId { get; set; }

        [ForeignKey("CursoId")]
        public Curso? Curso { get; set; }

        // Información adicional opcional
        public DateTime FechaPublicacion { get; set; } = DateTime.Now;

        public bool EsActivo { get; set; } = true;
    }
}
