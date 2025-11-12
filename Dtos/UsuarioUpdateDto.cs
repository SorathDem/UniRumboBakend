namespace UniRumboBakend.Dtos
{
    public class UsuarioUpdateDto
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string? Contrasena { get; set; }
        public int IdRol { get; set; }
        public int IdSede { get; set; }
    }
}
