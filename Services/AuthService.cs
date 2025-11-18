using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UniRumbo.Dtos;
using UniRumbo.Repositories;
using UniRumbo.Services.Interfaces;
using UniRumboBakend.Services.Interfaces;

namespace UniRumbo.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<UserResponseDto> RegisterAsync(RegisterDto dto)
        {
            Console.WriteLine($"DTO id_rol recibido: {dto.id_rol}");
            Console.WriteLine($"DTO id_sede recibido: {dto.id_sede}");
            // Validación de correo según rol
            if ((dto.id_rol == 1 || dto.id_rol == 2) && !dto.Correo.EndsWith("@ucundinamarca.edu.co"))
            {
                throw new Exception("Los usuarios y conductores deben registrarse con correo institucional (@ucundinamarca.edu.co)");
            }
            // Validación: correo único
            var existeCorreo = await _context.Usuario.AnyAsync(u => u.Correo == dto.Correo);
            if (existeCorreo)
            {
                throw new Exception("El correo ya está registrado");
            }
            // Validación: Rol y Sede deben existir en BD
            var rolExiste = await _context.Rol.AnyAsync(r => r.IdRol == dto.id_rol);
            if (!rolExiste) throw new Exception("El rol especificado no existe");
            var sedeExiste = await _context.Sede.AnyAsync(s => s.IdSede == dto.id_sede);
            if (!sedeExiste) throw new Exception("La sede especificada no existe");
            // Crear usuario
            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Numero = dto.Numero,
                Correo = dto.Correo,
                Contrasena = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena),
                IdSede = dto.id_sede,
                IdRol = dto.id_rol
            };
            try
            {
                _context.Usuario.Add(usuario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Error al guardar en la base de datos. Revisa que todos los campos y relaciones sean válidos.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado: {ex.Message}", ex);
            }
            return new UserResponseDto
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Correo = usuario.Correo,
                id_rol = usuario.IdRol
            };
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto dto)
        {
            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Correo == dto.Correo);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Contrasena, usuario.Contrasena))
            {
                return null; // Login inválido
            }

            // Generar el token JWT
            var token = GenerateJwtToken(usuario);

            // Opcional: incluir datos del usuario
            var userResponse = new UserResponseDto
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Correo = usuario.Correo,
                id_rol = usuario.IdRol
            };

            return new LoginResponseDto
            {
                Token = token,
                User = userResponse // Si solo quieres el token, pon User = null
            };
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Correo),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Role, usuario.IdRol.ToString()) // Rol para autorización
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpiresInMinutes"])),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private readonly IEmailService _emailService; // ← Añade esto también en el constructor

        public AuthService(ApplicationDbContext context, IConfiguration configuration, IEmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task RequestPasswordResetAsync(string correo)
        {
            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(u => u.Correo == correo);

            // Siempre respondemos igual aunque no exista el usuario (seguridad)
            if (usuario == null)
                return;

            // Generar token único
            var tokenPlano = Guid.NewGuid().ToString("N") + "-" + Guid.NewGuid().ToString("N");
            var hashedToken = BCrypt.Net.BCrypt.HashPassword(tokenPlano);

            usuario.ResetToken = hashedToken;
            usuario.ResetTokenExpires = DateTime.UtcNow.AddMinutes(30); // ← Usa la columna nullable

            await _context.SaveChangesAsync();

            // Enlace que abre tu frontend
            var resetLink = $"https://frontunirumbo.onrender.com/reset-password?token={Uri.EscapeDataString(tokenPlano)}&email={Uri.EscapeDataString(correo)}";

            await _emailService.SendPasswordResetEmailAsync(correo, resetLink);
        }

        public async Task<bool> ResetPasswordAsync(string email, string token, string nuevaContrasena)
        {
            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(u => u.Correo == email);

            if (usuario == null)
                return false;

            // Validar token y expiración
            if (usuario.ResetTokenExpires == null ||
                usuario.ResetTokenExpires < DateTime.UtcNow ||
                string.IsNullOrEmpty(usuario.ResetToken) ||
                !BCrypt.Net.BCrypt.Verify(token, usuario.ResetToken))
            {
                return false;
            }

            // Cambiar contraseña y limpiar token
            usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(nuevaContrasena);
            usuario.ResetToken = null;
            usuario.ResetTokenExpires = null;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}