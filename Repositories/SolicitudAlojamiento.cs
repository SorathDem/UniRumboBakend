using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniRumbo.Repositories
{
    [Table("Solicitud_alojamiento")]
    public partial class SolicitudAlojamiento
    {
        [Key]
        [Column("id_SoliAlojamiento")]
        public int IdSoliAlojamiento { get; set; }

        [Column("id_alojamiento")]
        public int IdAlojamiento { get; set; }

        [Column("id_estado")]
        public int IdEstado { get; set; }

        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        // 🔹 Relaciones
        [ForeignKey("IdEstado")]
        public virtual Estado Estado { get; set; } = null!;

        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; } = null!;

        [ForeignKey("IdAlojamiento")]
        public virtual Alojamiento Alojamiento { get; set; } = null!;
    }
}
