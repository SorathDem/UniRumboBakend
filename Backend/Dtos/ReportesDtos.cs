namespace UniRumbo.Backend.Dtos
{
    // DTO para generar reportes
    public class GenerarReporteDto
    {
        public string UsuarioNombre { get; set; } = string.Empty;
    }

    // DTO para datos de rutas más usadas
    public class RutaMasUsadaDto
    {
        public int Id { get; set; }
        public string Origen { get; set; } = string.Empty;
        public string Destino { get; set; } = string.Empty;
        public int CantidadUsos { get; set; }
        public int CantidadUsuarios { get; set; }
        public string DiaMasUsado { get; set; } = string.Empty;
        public decimal DistanciaKm { get; set; }
    }

    // DTO para datos de alojamientos
    public class AlojamientoReporteDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Sede { get; set; } = string.Empty;
        public int CapacidadTotal { get; set; }
        public int EspaciosOcupados { get; set; }
        public decimal PorcentajeOcupacion { get; set; }
        public string EstadoActual { get; set; } = string.Empty;
    }
}
