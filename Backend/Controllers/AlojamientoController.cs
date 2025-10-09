using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UniRumbo.Repositories;
using System.Web;

namespace UniRumboBakend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlojamientoController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly string _googleMapsApiKey = "TU_API_KEY"; // 🔑 coloca tu clave real de Google Maps aquí

        public AlojamientoController(ApplicationDbContext context)
        {
            _db = context;
        }

        // ✅ 1️⃣ LISTAR TODOS LOS ALOJAMIENTOS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var alojamientos = await _db.Alojamiento
                .Include(a => a.IdUsuarioNavigation)
                .ToListAsync();

            var result = alojamientos.Select(a => new
            {
                a.IdAlojamiento,
                a.Ubicacion,
                a.Descripcion,
                a.Id_Usuario,
                NombreUsuario = a.IdUsuarioNavigation?.Nombre,
                GoogleMapsUrl = GenerateMapsEmbedUrlFromUbicacion(a.Ubicacion)
            });

            return Ok(result);
        }

        // ✅ 2️⃣ OBTENER UN ALOJAMIENTO POR ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var a = await _db.Alojamiento
                .Include(x => x.IdUsuarioNavigation)
                .FirstOrDefaultAsync(x => x.IdAlojamiento == id);

            if (a == null)
                return NotFound(new { message = "Alojamiento no encontrado." });

            return Ok(new
            {
                a.IdAlojamiento,
                a.Ubicacion,
                a.Descripcion,
                a.Id_Usuario,
                NombreUsuario = a.IdUsuarioNavigation?.Nombre,
                GoogleMapsUrl = GenerateMapsEmbedUrlFromUbicacion(a.Ubicacion)
            });
        }

        // ✅ 3️⃣ CREAR ALOJAMIENTO CON COORDENADAS
        [HttpPost("con-coordenadas")]
        public async Task<IActionResult> CreateWithCoordinates([FromBody] AlojamientoWithCoordsDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string ubicacionString = $"lat:{dto.Latitud},lon:{dto.Longitud}";

            var alojamiento = new Alojamiento
            {
                Ubicacion = ubicacionString,
                Descripcion = dto.Descripcion,
                Id_Usuario = dto.Id_Usuario
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
        public async Task<IActionResult> Update(int id, [FromBody] AlojamientoUpdateDto dto)
        {
            var alojamiento = await _db.Alojamiento.FindAsync(id);
            if (alojamiento == null)
                return NotFound(new { message = "Alojamiento no encontrado." });

            // Si el cliente envía nuevas coordenadas, se actualiza la ubicación
            if (dto.Latitud != 0 && dto.Longitud != 0)
                alojamiento.Ubicacion = $"lat:{dto.Latitud},lon:{dto.Longitud}";

            alojamiento.Descripcion = dto.Descripcion ?? alojamiento.Descripcion;

            _db.Entry(alojamiento).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return Ok(new { message = "✅ Alojamiento actualizado correctamente." });
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
            string encoded = HttpUtility.UrlEncode(q);
            return $"https://www.google.com/maps/embed/v1/place?key={_googleMapsApiKey}&q={encoded}";
        }

        private string GenerateMapsEmbedUrlFromUbicacion(string ubicacion)
        {
            try
            {
                // Extrae lat/lon del texto: "lat:4.7,lon:-74.0"
                var partes = ubicacion.Replace("lat:", "").Replace("lon:", "").Split(',');
                double lat = double.Parse(partes[0]);
                double lon = double.Parse(partes[1]);
                return GenerateMapsEmbedUrl(lat, lon);
            }
            catch
            {
                return "";
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
    }

    public class AlojamientoUpdateDto
    {
        public string? Descripcion { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
    }
}
