using UniRumbo.Backend.Dtos;

namespace UniRumbo.Backend.Services.Interfaces;

public interface ISolicitudesRutaService {
  Task<int> CrearAsync(CrearSolicitudRutaDto dto);
  Task<IEnumerable<SolicitudListDto>> ObtenerMiasAsync(int idUsuario);
}
