using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VIMOD.Domain.Models
{
    public class Curso
    {
        [Key]
        public int IdCurso { get; set; }

        [Required]
        public string Nombre { get; set; }

        public string? Descripcion { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioDescuento { get; set; }

        public string? URLImagen { get; set; }

        public string? DocenteAsignado { get; set; }

        public int IdCategoria { get; set; }
        [ForeignKey("IdCategoria")]
        public Categoria? Categoria { get; set; }


        public virtual ICollection<ModuloCurso> Modulos { get; set; }
        public ICollection<PedidoDetalle> PedidoDetalles { get; set; }
        public ICollection<CarroCompras> CarritoCompras { get; set; } 
    }
}
