using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniRumbo.Repositories
{
    [Table("Alojamiento")]
    public class Alojamiento
    {
        [Key]
        [Column("id_alojamiento")]
        public int IdAlojamiento { get; set; }

        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Column("ubicacion")]
        public string? Ubicacion { get; set; }
        [Column("direccion")]
        public string? Direccion { get; set; }
        [Column("titulo")]
        public string? Titulo { get; set; }
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario? Usuario { get; set; }
        public virtual ICollection<Imagenes> Imagenes { get; set; }
    }

    public class Imagenes
    {
        [Key]
        [Column("id_img")]
        public int IdImg { get; set; }
        [Column("img")]
        public byte[]? Img { get; set; }
        [Column("id_alojamiento")]
        public int IdAlojamiento { get; set; }
        [ForeignKey("IdAlojamiento")]
        public virtual Alojamiento Alojamiento { get; set; }
    }

}
