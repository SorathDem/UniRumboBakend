using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniRumbo.Repositories;

public partial class Alojamiento
{
    [Key]
    public int IdAlojamiento { get; set; }

    public string Ubicacion { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int Id_Usuario { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<SolicitudAlojamiento> SolicitudAlojamientos { get; set; } = new List<SolicitudAlojamiento>();
}
