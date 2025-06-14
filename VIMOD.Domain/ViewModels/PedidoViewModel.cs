using System.ComponentModel.DataAnnotations;

namespace VIMOD.Domain.ViewModels
{
    public class PedidoViewModel
    {
        public int IdCurso { get; set; }
        public string NombreCurso { get; set; } = string.Empty;

        public decimal TotalPedido { get; set; }

        public string NombreUsuario { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string Direccion { get; set; } = string.Empty;
        public string CodigoPostal { get; set; } = string.Empty;
        public string Provincia { get; set; } = string.Empty;
        public string Localidad { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;

        public string FormaPago { get; set; } = string.Empty;

        // OPCIONAL, SOLO SI REALMENTE LO QUIERES PASAR DESDE LA VISTA
        public string? UserId { get; set; }
    }
}
