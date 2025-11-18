namespace UniRumboBakend.Dtos
{
    public class PasswordResetDto
    {
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string NuevaContrasena { get; set; } = null!;
    }
}
