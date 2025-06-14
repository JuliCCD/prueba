using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIMOD.Domain.Models
{
    public class CarroCompras
    {
        [Key]
        public int IdCarroCompras { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        public string UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }

        [Required]
        public int IdCurso { get; set; }
        [ForeignKey("IdCurso")]
        public Curso? Curso { get; set; }
    }
}
