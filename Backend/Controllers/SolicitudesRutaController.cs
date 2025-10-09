using Microsoft.AspNetCore.Mvc;
using UniRumbo.Backend.Dtos;
using UniRumbo.Backend.Services.Interfaces;

namespace UniRumbo.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SolicitudesRutaController : ControllerBase
{
  private readonly ISolicitudesRutaService _svc;
  public SolicitudesRutaController(ISolicitudesRutaService svc) { _svc = svc; }

  /// <summary>Crear solicitud a una ruta</summary>
  [HttpPost]
  public async Task<ActionResult> Crear([FromBody] CrearSolicitudRutaDto dto)
  {
    var id = await _svc.CrearAsync(dto);
    return CreatedAtAction(nameof(GetMias), new { idUsuario = dto.IdUsuario }, new { id });
  }

  /// <summary>Ver mis solicitudes</summary>
  [HttpGet("mias/{idUsuario:int}")]
  public async Task<ActionResult<IEnumerable<SolicitudListDto>>> GetMias(int idUsuario)
    => Ok(await _svc.ObtenerMiasAsync(idUsuario));
}
