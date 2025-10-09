using Microsoft.EntityFrameworkCore;
using UniRumbo.Backend.Dtos;
using UniRumbo.Repositories;

namespace UniRumbo.Backend.Services;

public class SolicitudesRutaService : Interfaces.ISolicitudesRutaService
{
  private readonly ApplicationDbContext _db;
  public SolicitudesRutaService(ApplicationDbContext db) { _db = db; }

  public async Task<int> CrearAsync(CrearSolicitudRutaDto dto)
  {
  var s = new UniRumbo.Backend.Repositories.Entities.SolicitudRuta {
      IdUsuario = dto.IdUsuario, IdRuta = dto.IdRuta, IdEstado = dto.IdEstado
    };
  _db.SolicitudesRuta.Add(s);
    await _db.SaveChangesAsync();
    return s.IdSoliRuta;
  }

  public async Task<IEnumerable<SolicitudListDto>> ObtenerMiasAsync(int idUsuario)
  {
  var list = await _db.SolicitudesRuta.Include(x => x.Estado)
      .Where(x => x.IdUsuario == idUsuario).OrderByDescending(x => x.IdSoliRuta).ToListAsync();

  return list.Select(x => new SolicitudListDto(x.IdSoliRuta, x.IdRuta, x.Estado?.Estado1 ?? "Pendiente", DateTime.UtcNow));
  }
}
