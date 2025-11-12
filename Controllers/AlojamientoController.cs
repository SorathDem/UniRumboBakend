using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Web;
using UniRumbo.Repositories;
using UniRumbo.Services;
using UniRumboBakend.Dtos;

namespace UniRumboBakend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlojamientoController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly string _googleMapsApiKey = "AIzaSyCCdUNE22w7v-BO8h9HZmcQuP1iz6jlOfA"; // 🔑 coloca tu clave real de Google Maps aquí

        public AlojamientoController(ApplicationDbContext context)
        {
            _db = context;
        }

        // ✅ 1️⃣ LISTAR TODOS LOS ALOJAMIENTOS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var alojamientos = await _db.Alojamiento
                .Include(a => a.Usuario)
                .Include(a => a.Imagenes)
                .ToListAsync();

            var result = alojamientos.Select(a => new
            {
                a.IdAlojamiento,
                a.Ubicacion,
                a.Descripcion,
                a.Direccion,
                a.Titulo,
                a.IdUsuario,
                NombreUsuario = a.Usuario?.Nombre,
                Imagenes = a.Imagenes
             .Where(i => i.Img != null)
             .Select(i => Convert.ToBase64String(i.Img)),
                GoogleMapsUrl = GenerateMapsEmbedUrlFromUbicacion(a.Ubicacion)
            });

            return Ok(result);
        }

        // ✅ 2️⃣ OBTENER UN ALOJAMIENTO POR ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var a = await _db.Alojamiento
                .Include(x => x.Usuario)
                .Include(x => x.Imagenes)
                .FirstOrDefaultAsync(x => x.IdAlojamiento == id);

            if (a == null)
                return NotFound(new { message = "Alojamiento no encontrado." });

            return Ok(new
            {
                a.IdAlojamiento,
                a.Ubicacion,
                a.Descripcion,
                a.Direccion,
                a.Titulo,
                a.IdUsuario,
                NombreUsuario = a.Usuario?.Nombre,
                Imagenes = a.Imagenes
             .Where(i => i.Img != null)
             .Select(i => Convert.ToBase64String(i.Img)),
                GoogleMapsUrl = GenerateMapsEmbedUrlFromUbicacion(a.Ubicacion)
            });
        }

        // ✅ 3️⃣ CREAR ALOJAMIENTO CON COORDENADAS
        [HttpPost("con-coordenadas")]
        public async Task<IActionResult> CreateWithCoordinates([FromBody] AlojamientoWithCoordsDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Usa un usuario por defecto si no se envía ninguno
            int usuarioId = dto.Id_Usuario != 0 ? dto.Id_Usuario : 1;

            // Verifica que el usuario exista
            var usuario = await _db.Usuario.FindAsync(usuarioId);
            if (usuario == null)
                return BadRequest("El usuario especificado no existe.");

            string ubicacionString = $"lat:{dto.Latitud},lon:{dto.Longitud}";

            var alojamiento = new Alojamiento
            {
                Ubicacion = ubicacionString,
                Descripcion = dto.Descripcion,
                Direccion = dto.Direccion,
                Titulo = dto.Titulo,
                IdUsuario = usuarioId
            };

            await _db.Alojamiento.AddAsync(alojamiento);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "✅ Alojamiento creado con coordenadas correctamente.",
                ubicacion = ubicacionString,
                mapa = GenerateMapsEmbedUrl(dto.Latitud, dto.Longitud)
            });
        }


        // ✅ 4️⃣ EDITAR / ACTUALIZAR ALOJAMIENTO
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AlojamientoEditDto dto)
        {
            var alojamiento = await _db.Alojamiento.FindAsync(id);
            if (alojamiento == null)
                return NotFound(new { message = "Alojamiento no encontrado." });

            // Solo actualizar los campos editables
            alojamiento.Titulo = dto.Titulo ?? alojamiento.Titulo;
            alojamiento.Direccion = dto.Direccion ?? alojamiento.Direccion;
            alojamiento.Descripcion = dto.Descripcion ?? alojamiento.Descripcion;

            _db.Entry(alojamiento).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return Ok(new { message = "✅ Alojamiento actualizado correctamente." });
        }



        [HttpPost("con-imagenes")]
        public async Task<IActionResult> CreateWithImages([FromBody] AlojamientoWithImagesDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var alojamiento = new Alojamiento
            {
                Ubicacion = $"lat:{dto.Latitud},lon:{dto.Longitud}",
                Descripcion = dto.Descripcion,
                IdUsuario = dto.Id_Usuario,
                Direccion = dto.Direccion,
                Titulo = dto.Titulo,
                Imagenes = dto.ImagenesBase64.Select(base64 => new Imagenes
                {
                    Img = Convert.FromBase64String(base64)
                }).ToList()
            };

            await _db.Alojamiento.AddAsync(alojamiento);
            await _db.SaveChangesAsync();

            return Ok(new { message = "✅ Alojamiento creado con imágenes correctamente." });
        }

        // ✅ 5️⃣ ELIMINAR ALOJAMIENTO
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var alojamiento = await _db.Alojamiento.FindAsync(id);
            if (alojamiento == null)
                return NotFound(new { message = "Alojamiento no encontrado." });

            _db.Alojamiento.Remove(alojamiento);
            await _db.SaveChangesAsync();

            return Ok(new { message = "🗑️ Alojamiento eliminado correctamente." });
        }

        // 🔧 FUNCIONES AUXILIARES
        private string GenerateMapsEmbedUrl(double lat, double lon)
        {
            string q = $"{lat},{lon}";
            return $"https://www.google.com/maps/embed/v1/place?key={_googleMapsApiKey}&q={q}&zoom=15";
        }


        private string GenerateMapsEmbedUrlFromUbicacion(string ubicacion)
        {
            try
            {
                var partes = ubicacion.Replace("lat:", "").Replace("lon:", "").Split(',');
                double lat = double.Parse(partes[0]);
                double lon = double.Parse(partes[1]);
                return GenerateMapsEmbedUrl(lat, lon);
            }
            catch
            {
                return null;
            }
        }

    }

    // 🧾 DTOs
    public class AlojamientoWithCoordsDto
    {
        public string Descripcion { get; set; } = null!;
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public int Id_Usuario { get; set; }
        public string? Direccion { get; set; }
        public string? Titulo { get; set; }
    }

    public class AlojamientoUpdateDto
    {
        public string? Descripcion { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public int IdUsuario { get; internal set; }
        public string Ubicacion { get; internal set; }
        public string? Direccion { get; set; }
        public string? Titulo { get; set; }
    }
}
