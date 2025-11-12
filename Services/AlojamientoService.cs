using UniRumbo.Dtos;
using UniRumbo.Repositories;
using UniRumboBakend.Controllers;
using UniRumboBakend.Dtos;

namespace UniRumbo.Services
{
    public class AlojamientoService
    {
        private readonly AlojamientoRepository _repository;

        public AlojamientoService(AlojamientoRepository repository)
        {
            _repository = repository;
        }

        // ✅ Listar todos
        public async Task<List<AlojamientoDto>> GetAllAsync()
        {
            var alojamientos = await _repository.GetAllAsync();
            return alojamientos.Select(a => new AlojamientoDto
            {
                IdAlojamiento = a.IdAlojamiento,
                Ubicacion = a.Ubicacion,
                Descripcion = a.Descripcion,
                Id_Usuario = a.IdUsuario,
                Direccion = a.Direccion,
                Titulo = a.Titulo
            }).ToList();
        }

        // ✅ Obtener por ID
        public async Task<AlojamientoDto?> GetByIdAsync(int id)
        {
            var a = await _repository.GetByIdAsync(id);
            if (a == null) return null;

            return new AlojamientoDto
            {
                IdAlojamiento = a.IdAlojamiento,
                Ubicacion = a.Ubicacion,
                Descripcion = a.Descripcion,
                Id_Usuario = a.IdUsuario,
                Direccion = a.Direccion,
                Titulo =a.Titulo
            };
        }

        // ✅ Crear
        public async Task AddAsync(AlojamientoCreateDto dto)
        {
            var alojamiento = new Alojamiento
            {
                Ubicacion = dto.Ubicacion,
                Descripcion = dto.Descripcion,
                IdUsuario = dto.Id_Usuario,
                Direccion = dto.Direccion,
                Titulo = dto.Titulo
            };

            await _repository.AddAsync(alojamiento);
        }

        // ✅ Actualizar
        public async Task<bool> UpdateAsync(int id, AlojamientoEditDto dto)
        {
            var alojamiento = await _repository.GetByIdAsync(id);
            if (alojamiento == null) return false;

            
            alojamiento.Descripcion = dto.Descripcion;
            alojamiento.Direccion = dto.Direccion;
            alojamiento.Titulo = dto.Titulo;

            await _repository.UpdateAsync(alojamiento);
            return true;
        }

        // ✅ Eliminar
        public async Task<bool> DeleteAsync(int id)
        {
            var alojamiento = await _repository.GetByIdAsync(id);
            if (alojamiento == null) return false;

            await _repository.DeleteAsync(alojamiento);
            return true;
        }
    }
}
