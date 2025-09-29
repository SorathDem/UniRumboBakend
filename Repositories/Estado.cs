using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniRumbo.Repositories;

public partial class Estado
{
    [Key]
    public int IdEstado { get; set; }

    public string Estado1 { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<SolicitudAlojamiento> SolicitudAlojamientos { get; set; } = new List<SolicitudAlojamiento>();

    public virtual ICollection<SolicitudRutum> SolicitudRuta { get; set; } = new List<SolicitudRutum>();
}
