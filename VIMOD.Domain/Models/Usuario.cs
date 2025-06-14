using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace VIMOD.Domain.Models
{
    public class Usuario : IdentityUser
    {
        [Required(ErrorMessage = "Nombres es requerido")]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Apellidos es requerido")]
        [MaxLength(100)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Correo es requerido")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        public string Direccion { get; set; }

        public string Telefono { get; set; }

        [Required(ErrorMessage = "Fecha de nacimiento es requerida")]
        public DateTime FechaDeNacimiento { get; set; }

        public ICollection<Pedido>? Pedidos { get; set; }

        public ICollection<CarroCompras>? CarritoCompras { get; set; }
    }
}
