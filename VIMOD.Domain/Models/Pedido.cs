using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIMOD.Domain.Models
{
    public class Pedido
    {
        [Key]
        public int IdPedido { get; set; }

        public DateTime FechaPedido { get; set; }
        public string FormaPago { get; set; }
        public string EstadoPedido { get; set; }
        public decimal TotalPedido { get; set; }

        public string Direccion { get; set; }
        public string CodigoPostal { get; set; }
        public string Provincia { get; set; }
        public string Localidad { get; set; }
        public string Telefono { get; set; }

        public string UserId { get; set; }
        
        [ForeignKey("UserId")]
        public Usuario? IdUsuario { get; set; }

        public ICollection<PedidoDetalle> Detalles { get; set; }
    }
}
