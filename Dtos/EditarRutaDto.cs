namespace UniRumboBakend.Dtos
{
    public class EditarRutaDto
    {
        public DateTime? HoraSalida { get; set; }
        public DateTime? HoraRegreso { get; set; }
        public string? DesVehiculo { get; set; }
        public int? CuposIda { get; set; }
        public int? CuposVuelta { get; set; }
        public int? IdVehiculo { get; set; }
        public string? DiasRuta { get; set; }
    }
}
