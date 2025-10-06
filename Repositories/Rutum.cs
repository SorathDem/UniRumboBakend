using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniRumbo.Repositories;

public partial class Rutum
{
    [Key]
    public int IdRuta { get; set; }

    public string PuntoOrigen { get; set; } = null!;

    public string PuntoDestino { get; set; } = null!;

    public DateTime HoraSalida { get; set; }

    public DateTime? HoraRegreso { get; set; }

    public int CuposIda { get; set; }

    public int? CuposVuelta { get; set; }

    public int IdUsuario { get; set; }

    public int IdVehiculo { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual Vehiculo IdVehiculoNavigation { get; set; } = null!;

    public virtual ICollection<SolicitudRutum> SolicitudRuta { get; set; } = new List<SolicitudRutum>();
}
