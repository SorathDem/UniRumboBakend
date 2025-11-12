using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniRumbo.Repositories;

[Table("Vehiculo")]
public partial class Vehiculo
{
    [Key]
    [Column("id_vehiculo")]
    public int IdVehiculo { get; set; }

    [Column("tipo_vehiculo")]
    public bool TipoVehiculo { get; set; }

    public ICollection<Rutum> Rutas { get; set; } = new List<Rutum>();
}

