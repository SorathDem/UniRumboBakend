using UniRumbo.Repositories;

namespace UniRumbo.Backend.Repositories.Entities;

public class Ruta
{
    public int IdRuta { get; set; }
    public string PuntoOrigen { get; set; } = null!;
    public string PuntoDestino { get; set; } = null!;
    public DateTime HoraSalida { get; set; }
    public DateTime? HoraRegreso { get; set; }
    public int CuposIda { get; set; }
    public int? CuposVuelta { get; set; }
    public int IdUsuario { get; set; }
    public int IdVehiculo { get; set; }

    // opcional para el trazo del mapa (recomendado)
    public double? OrigenLat { get; set; }
    public double? OrigenLon { get; set; }
    public double? DestinoLat { get; set; }
    public double? DestinoLon { get; set; }

    public Usuario? Usuario { get; set; }
    public Vehiculo? Vehiculo { get; set; }
    public ICollection<SolicitudRuta> Solicitudes { get; set; } = new List<SolicitudRuta>();
}
