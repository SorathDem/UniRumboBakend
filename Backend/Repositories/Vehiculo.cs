using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UniRumbo.Backend.Repositories.Entities;

namespace UniRumbo.Repositories;

public partial class Vehiculo
{
    [Key]
    public int IdVehiculo { get; set; }

    public bool TipoVehiculo { get; set; }

    public virtual ICollection<Ruta> Ruta { get; set; } = new List<Ruta>();
}
