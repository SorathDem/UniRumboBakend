using UniRumbo.Backend.Dtos;

namespace UniRumbo.Backend.Services.Interfaces;

public interface IRutasService {
  Task<IEnumerable<RutaListDto>> BuscarAsync(string? origen, string? destino, DateTime? fecha);
  Task<RutaListDto?> ObtenerPorIdAsync(int id);
  Task<int> CrearAsync(CrearRutaDto dto);
  Task<IEnumerable<RutaListDto>> ObtenerMiasAsync(int usuarioId);
  Task<bool> EditarAsync(EditarRutaDto dto);
}
