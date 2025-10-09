using Microsoft.AspNetCore.Mvc;
using UniRumbo.Backend.Dtos;
using UniRumbo.Backend.Services.Interfaces;

namespace UniRumbo.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class RutasController : ControllerBase
{
  private readonly IRutasService _svc;
  private readonly IHttpClientFactory _http;
  public RutasController(IRutasService svc, IHttpClientFactory http) { _svc = svc; _http = http; }

  /// <summary>Buscar rutas por origen/destino/fecha</summary>
  [HttpGet]
  public async Task<ActionResult<IEnumerable<RutaListDto>>> Buscar([FromQuery] string? origen, [FromQuery] string? destino, [FromQuery] DateTime? fecha)
    => Ok(await _svc.BuscarAsync(origen, destino, fecha));

  /// <summary>Crear una nueva ruta</summary>
  [HttpPost]
  public async Task<ActionResult<int>> Crear([FromBody] CrearRutaDto dto)
  {
    try
    {
      var id = await _svc.CrearAsync(dto);
      return CreatedAtAction(nameof(GetById), new { id }, id);
    }
    catch (Exception ex)
    {
      return BadRequest(new { message = ex.GetBaseException().Message });
    }
  }

  /// <summary>Detalle de una ruta</summary>
  [HttpGet("{id:int}")]
  public async Task<ActionResult<RutaListDto>> GetById(int id)
  {
    var r = await _svc.ObtenerPorIdAsync(id);
    return r is null ? NotFound() : Ok(r);
  }

  /// <summary>GeoJSON del recorrido (línea + marcadores)</summary>
  [HttpGet("{id:int}/geo")]
  public async Task<ActionResult<object>> Geo(int id)
  {
    var fc = await _svc.GeoAsync(id, _http);
    return fc is null ? NotFound("Esta ruta no tiene coordenadas.") : Ok(fc);
  }
}
