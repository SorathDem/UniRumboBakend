namespace UniRumbo.Dtos
{
    public class RegisterDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public required string Numero { get; set; }
        public string Correo { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public int id_sede { get; set; }
        public int id_rol { get; set; }
    }
}
