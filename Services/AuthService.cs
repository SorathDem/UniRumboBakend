using Microsoft.EntityFrameworkCore;
using UniRumbo.Dtos;
using UniRumbo.Repositories;
using UniRumbo.Services.Interfaces;

namespace UniRumbo.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserResponseDto> RegisterAsync(RegisterDto dto)
        {
            Console.WriteLine($"DTO id_rol recibido: {dto.id_rol}");
            Console.WriteLine($"DTO id_sede recibido: {dto.id_sede}");

            // ⚡ Validación de correo según rol
            if ((dto.id_rol == 1 || dto.id_rol == 2) && !dto.Correo.EndsWith("@ucundinamarca.edu.co"))
            {
                throw new Exception("Los usuarios y conductores deben registrarse con correo institucional (@ucundinamarca.edu.co)");
            }

            // ⚡ Validación: correo único
            var existeCorreo = await _context.Usuario.AnyAsync(u => u.Correo == dto.Correo);
            if (existeCorreo)
            {
                throw new Exception("El correo ya está registrado");
            }

            // ⚡ Validación: Rol y Sede deben existir en BD
            var rolExiste = await _context.Rol.AnyAsync(r => r.IdRol == dto.id_rol);
            if (!rolExiste) throw new Exception("El rol especificado no existe");

            var sedeExiste = await _context.Sede.AnyAsync(s => s.IdSede == dto.id_sede);
            if (!sedeExiste) throw new Exception("La sede especificada no existe");


            // ⚡ Crear usuario
            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Numero = dto.Numero,
                Correo = dto.Correo,
                Contrasena = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena), // encriptamos
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
                // Error típico: clave foránea o constraint
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
                id_rol = usuario.IdRol,
           
            };
        }

        public async Task<UserResponseDto?> LoginAsync(LoginDto dto)
        {
            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Correo == dto.Correo);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Contrasena, usuario.Contrasena))
            {
                return null; // login inválido
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
    }
}
