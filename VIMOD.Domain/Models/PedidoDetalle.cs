using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIMOD.Domain.Models
{
    public class PedidoDetalle
    {
        [Key]
        public int IdDetallePedido { get; set; }

        public int Cantidad { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioIndividual { get; set; }

        public int IdPedido { get; set; }
        [ForeignKey("IdPedido")]
        public Pedido? Pedido { get; set; }

        public int IdCurso { get; set; }
        [ForeignKey("IdCurso")]
        public Curso? Curso { get; set; }
    }

}
