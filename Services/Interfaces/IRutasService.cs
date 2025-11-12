using UniRumbo.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniRumbo.Services.Interfaces
{
    public interface IRutasService
    {
        Task<IEnumerable<RutaListDto>> GetRutasAsync();
        Task<RutaListDto?> GetRutaByIdAsync(int id);
        Task<RutaListDto> CrearRutaAsync(CrearRutaDto dto);
        Task<RutaListDto?> EditarRutaAsync(int id, EditarRutaDto dto);
        Task<bool> EliminarRutaAsync(int id);
    }
}
