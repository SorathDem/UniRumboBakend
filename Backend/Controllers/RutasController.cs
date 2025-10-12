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
  public RutasController(IRutasService svc) { _svc = svc; }

  // Eliminado: listado público. Nos quedamos con Crear, Editar, Mis Rutas y Detalle.

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

  /// <summary>Editar una ruta</summary>
  [HttpPut]
  public async Task<ActionResult> Editar([FromBody] EditarRutaDto dto)
  {
    var ok = await _svc.EditarAsync(dto);
    return ok ? NoContent() : NotFound();
  }

  /// <summary>Listar solo mis rutas</summary>
  [HttpGet("mias/{usuarioId:int}")]
  public async Task<ActionResult<IEnumerable<RutaListDto>>> Mias(int usuarioId)
    => Ok(await _svc.ObtenerMiasAsync(usuarioId));

  /// <summary>Detalle de una ruta</summary>
  [HttpGet("{id:int}")]
  public async Task<ActionResult<RutaListDto>> GetById(int id)
  {
    var r = await _svc.ObtenerPorIdAsync(id);
    return r is null ? NotFound() : Ok(r);
  }

  // Eliminado endpoint de GeoJSON: fuera de alcance actual.
}
