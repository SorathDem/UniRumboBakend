namespace UniRumbo.Dtos
{
    public class UserResponseDto
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public int id_rol { get; set; }
        public int IdSede { get; set; }
        public string Token { get; set; }
    }
}
