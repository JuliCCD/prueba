using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIMOD.Domain.Models
{
    public class Carousel
    {
        public int Id { get; set; }

        [Required]      
        [StringLength(100)]
        public string Titulo { get; set; }

        [StringLength(200)]
        public string Descripcion { get; set; }

        [Display(Name = "Imagen")]
        public string ImagenUrl { get; set; }

        public bool Activo { get; set; } = true;
        public int Orden { get; set; } = 0;
    }
}
