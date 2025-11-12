namespace UniRumboBakend.Dtos
{
    public class AlojamientoWithImagesDto
    {
        public string Descripcion { get; set; } = null!;
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public int Id_Usuario { get; set; }
        public string? Direccion { get; set; }
        public string? Titulo { get; set; }
        public List<string> ImagenesBase64 { get; set; } = new();
    }
}