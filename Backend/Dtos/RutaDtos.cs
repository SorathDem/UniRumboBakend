namespace UniRumbo.Backend.Dtos;

public record RutaListDto(
    int Id, string Titulo, string Estado,
    string Origen, string Destino, string TipoVehiculo,
    int Cupos, DateTime HoraSalida, DateTime? HoraRegreso);

public class CrearRutaDto {
    public string PuntoOrigen { get; set; } = null!;
    public string PuntoDestino { get; set; } = null!;
    public DateTime HoraSalida { get; set; }
    public DateTime? HoraRegreso { get; set; }
    public int CuposIda { get; set; }
    public int? CuposVuelta { get; set; }
    public int IdUsuario { get; set; }
    public int IdVehiculo { get; set; }
    public double? OrigenLat { get; set; }
    public double? OrigenLon { get; set; }
    public double? DestinoLat { get; set; }
    public double? DestinoLon { get; set; }
}
public class EditarRutaDto : CrearRutaDto { public int IdRuta { get; set; } }
