using Microsoft.AspNetCore.Mvc;
using UniRumbo.Dtos;
using UniRumbo.Services.Interfaces;
using System.Threading.Tasks;

namespace UniRumbo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class RutasController : ControllerBase
    {
        private readonly IRutasService _rutasService;

        public RutasController(IRutasService rutasService)
        {
            _rutasService = rutasService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRutas()
        {
            var rutas = await _rutasService.GetRutasAsync();
            return Ok(rutas);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetRuta(int id)
        {
            var ruta = await _rutasService.GetRutaByIdAsync(id);
            if (ruta == null) return NotFound();
            return Ok(ruta);
        }


        [HttpPost]
        public async Task<IActionResult> CrearRuta([FromBody] CrearRutaDto dto)
        {
            var ruta = await _rutasService.CrearRutaAsync(dto);
            return CreatedAtAction(nameof(GetRuta), new { id = ruta.IdRuta }, ruta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditarRuta(int id, [FromBody] EditarRutaDto dto)
        {
            var ruta = await _rutasService.EditarRutaAsync(id, dto);
            if (ruta == null) return NotFound();
            return Ok(ruta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarRuta(int id)
        {
            var result = await _rutasService.EliminarRutaAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("geocode")]  // 👈 ruta explícita para evitar confusión con {id}
        public async Task<IActionResult> Geocode([FromQuery] string lugar)
        {
            if (string.IsNullOrWhiteSpace(lugar))
                return BadRequest("El parámetro 'lugar' es obligatorio.");

            try
            {
                using var client = new HttpClient();

                // Encodifica el nombre del lugar
                var url = $"https://nominatim.openstreetmap.org/search?format=json&q={Uri.EscapeDataString(lugar)}";

                // User-Agent obligatorio para Nominatim
                client.DefaultRequestHeaders.Add("User-Agent", "UniRumboApp/1.0 (contacto@unirumbo.com)");

                var response = await client.GetStringAsync(url);
                return Content(response, "application/json");
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Error al consultar Nominatim: {ex.Message}");
            }
        }
    }
}
