using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniRumbo.Repositories;

public partial class Vehiculo
{
    [Key]
    public int IdVehiculo { get; set; }

    public bool TipoVehiculo { get; set; }

    public virtual ICollection<Rutum> Ruta { get; set; } = new List<Rutum>();
}
