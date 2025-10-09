using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniRumbo.Repositories;

public partial class SolicitudAlojamiento
{
    [Key]
    public int IdSoliAlojamiento { get; set; }

    public int IdAlojamiento { get; set; }

    public int IdEstado { get; set; }

    public int IdUsuario { get; set; }

    public virtual Alojamiento IdAlojamientoNavigation { get; set; } = null!;

    public virtual Estado IdEstadoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
