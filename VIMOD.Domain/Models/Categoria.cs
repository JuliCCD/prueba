using System.Text.Json.Serialization;

namespace VIMOD.Domain.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }

        // Relación de uno a muchos
        [JsonIgnore]
        public ICollection<Curso>? Cursos { get; set; }
    }
}
