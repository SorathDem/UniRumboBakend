using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniRumbo.Repositories;

public partial class SolicitudRutum
{
    [Key]
    public int IdSoliRuta { get; set; }

    public int IdEstado { get; set; }

    public int IdUsuario { get; set; }

    public int IdRuta { get; set; }

    public virtual Estado IdEstadoNavigation { get; set; } = null!;

    public virtual Rutum IdRutaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
