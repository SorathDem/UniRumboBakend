public record RutaListDto(
    int IdRuta,
    string PuntoOrigen,
    string PuntoDestino,
    DateTime HoraSalida,
    DateTime HoraRegreso,
    string DesVehiculo,
    int CuposIda,
    int CuposVuelta,
    int IdUsuario,
    int IdVehiculo,
    string DiasRuta
);

public class CrearRutaDto
{
    public string PuntoOrigen { get; set; } = null!;
    public string PuntoDestino { get; set; } = null!;
    public DateTime HoraSalida { get; set; }
    public DateTime HoraRegreso { get; set; }
    public string? DesVehiculo { get; set; }
    public int CuposIda { get; set; }
    public int CuposVuelta { get; set; }
    public int IdUsuario { get; set; }
    public int IdVehiculo { get; set; }
    public string? DiasRuta { get; set; }
}

public class EditarRutaDto : CrearRutaDto
{
    public int IdRuta { get; set; }
}
