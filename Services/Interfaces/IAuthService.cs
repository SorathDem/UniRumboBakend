using UniRumbo.Dtos;
using UniRumboBakend.Dtos;

namespace UniRumbo.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponseDto> RegisterAsync(RegisterDto dto);
        Task<LoginResponseDto?> LoginAsync(LoginDto dto);

        Task RequestPasswordResetAsync(string correo);
        Task<bool> ResetPasswordAsync(string email, string token, string nuevaContrasena);
    }
}
