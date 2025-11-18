using Microsoft.AspNetCore.Mvc;
using UniRumbo.Dtos;
using UniRumbo.Services.Interfaces;
using UniRumboBakend.Dtos;

namespace UniRumbo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // Tus endpoints existentes
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var result = await _authService.RegisterAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("login")] //Comunicacion con el Login y el inicio de sesion
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var response = await _authService.LoginAsync(dto);
            if (response == null)
                return Unauthorized(new { mensaje = "Credenciales incorrectas" });
            return Ok(response);
        }

        // ← NUEVO: Solicitar recuperación de contraseña
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] PasswordResetRequestDto dto)
        {
            if (string.IsNullOrEmpty(dto.Correo))
                return BadRequest(new { mensaje = "El correo es requerido" });

            await _authService.RequestPasswordResetAsync(dto.Correo);

            // Siempre la misma respuesta (seguridad)
            return Ok(new { mensaje = "Si el correo está registrado, recibirás un enlace para restablecer tu contraseña." });
        }

        // ← NUEVO: Cambiar contraseña con token
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetDto dto)
        {
            if (string.IsNullOrEmpty(dto.Email) ||
                string.IsNullOrEmpty(dto.Token) ||
                string.IsNullOrEmpty(dto.NuevaContrasena))
            {
                return BadRequest(new { mensaje = "Todos los campos son requeridos" });
            }

            var resultado = await _authService.ResetPasswordAsync(dto.Email, dto.Token, dto.NuevaContrasena);

            if (!resultado)
                return BadRequest(new { mensaje = "Token inválido o expirado" });

            return Ok(new { mensaje = "¡Contraseña actualizada correctamente!" });
        }
    }
}