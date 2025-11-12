using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniRumbo.Repositories
{
    [Table("Estado")]
    public partial class Estado
    {
        [Key]
        [Column("id_estado")]
        public int IdEstado { get; set; }

        [Column("estado")]
        public string EstadoNombre { get; set; } = null!;

        [Column("descripcion")]
        public string Descripcion { get; set; } = null!;

        // 🔹 Relaciones inversas
        public virtual ICollection<SolicitudRutum> SolicitudRuta { get; set; } = new List<SolicitudRutum>();
        public virtual ICollection<SolicitudAlojamiento> SolicitudAlojamiento { get; set; } = new List<SolicitudAlojamiento>();
    }
}
