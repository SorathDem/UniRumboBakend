using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniRumbo.Repositories
{
    [Table("Solicitud_ruta")]
    public partial class SolicitudRutum
    {
        [Key]
        [Column("id_SoliRuta")]
        public int IdSoliRuta { get; set; }

        [Column("id_estado")]
        public int IdEstado { get; set; }

        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("id_ruta")]
        public int IdRuta { get; set; }

        // 🔹 Relaciones
        [ForeignKey("IdEstado")]
        public virtual Estado Estado { get; set; } = null!;

        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; } = null!;

        [ForeignKey("IdRuta")]
        public virtual Rutum Ruta { get; set; } = null!;
    }
}
