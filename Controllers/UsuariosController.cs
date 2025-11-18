using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniRumbo.Dtos; // Para UserResponseDto
using UniRumbo.Repositories;
using UniRumboBakend.Dtos;
//Controlador de Usuarios para gestionar operaciones CRUD, sirve para administrar la informacion de los usuarios
namespace UniRumbo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsuarios()
        {
            var usuarios = await _context.Usuario
                .Select(u => new UserResponseDto
                {
                    IdUsuario = u.IdUsuario,
                    Nombre = u.Nombre,
                    Apellido = u.Apellido,
                    Correo = u.Correo,
                    id_rol = u.IdRol,
                    IdSede = u.IdSede
                })
                .ToListAsync();

            return Ok(usuarios);
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUsuario(int id)
        {
            var usuario = await _context.Usuario
                .Where(u => u.IdUsuario == id)
                .Select(u => new UserResponseDto
                {
                    IdUsuario = u.IdUsuario,
                    Nombre = u.Nombre,
                    Apellido = u.Apellido,
                    Correo = u.Correo,
                    id_rol = u.IdRol
                })
                .FirstOrDefaultAsync();

            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            // Hash de la contraseña
            usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasena);

            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.IdUsuario }, usuario);
        }

        // PUT: api/Usuarios/EditarCompleto/5
        [HttpPut("EditarCompleto/{id}")]
        public async Task<IActionResult> PutUsuarioCompleto(int id, [FromBody] UsuarioUpdateDto usuarioActualizado)
        {
            if (usuarioActualizado == null)
                return BadRequest("Los datos del usuario son requeridos.");

            var usuarioExistente = await _context.Usuario.FindAsync(id);
            if (usuarioExistente == null)
                return NotFound();

            try
            {
                usuarioExistente.Nombre = usuarioActualizado.Nombre;
                usuarioExistente.Apellido = usuarioActualizado.Apellido;
                usuarioExistente.Numero = usuarioActualizado.Numero;
                usuarioExistente.Correo = usuarioActualizado.Correo;
                usuarioExistente.IdRol = usuarioActualizado.IdRol;
                usuarioExistente.IdSede = usuarioActualizado.IdSede;

                if (!string.IsNullOrWhiteSpace(usuarioActualizado.Contrasena))
                {
                    usuarioExistente.Contrasena = BCrypt.Net.BCrypt.HashPassword(usuarioActualizado.Contrasena);
                }

                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "Usuario actualizado correctamente ✅" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar usuario", detalle = ex.Message });
            }
        }


        // PUT: api/Usuarios/5
        // Solo actualizar rol y sede
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, [FromBody] UpdateUsuarioDto dto)
        {
            var usuarioExistente = await _context.Usuario.FindAsync(id);
            if (usuarioExistente == null)
                return NotFound();

            // Actualizar solo rol y sede
            usuarioExistente.IdRol = dto.IdRol;
            usuarioExistente.IdSede = dto.IdSede;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
                return NotFound();

            _context.Usuario.Remove(usuario);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
