using UniRumbo.Dtos;

namespace UniRumbo.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponseDto> RegisterAsync(RegisterDto dto);
        Task<UserResponseDto?> LoginAsync(LoginDto dto);
    }
}
