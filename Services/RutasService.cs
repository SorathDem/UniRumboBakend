using Microsoft.EntityFrameworkCore;
using UniRumbo.Dtos;
using UniRumbo.Repositories;
using UniRumbo.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniRumbo.Services
{
    public class RutasService : IRutasService
    {
        private readonly ApplicationDbContext _context;

        public RutasService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RutaListDto>> GetRutasAsync()
        {
            return await _context.Rutas
                .Select(r => new RutaListDto(
                    r.IdRuta,
                    r.PuntoOrigen,
                    r.PuntoDestino,
                    r.HoraSalida,
                    r.HoraRegreso,
                    r.DesVehiculo,
                    r.CuposIda,
                    r.CuposVuelta,
                    r.IdUsuario,
                    r.IdVehiculo,
                    r.DiasRuta
                ))
                .ToListAsync();
        }

        public async Task<RutaListDto?> GetRutaByIdAsync(int id)
        {
            var ruta = await _context.Rutas.FindAsync(id);
            if (ruta == null) return null;

            return new RutaListDto(
                ruta.IdRuta,
                ruta.PuntoOrigen,
                ruta.PuntoDestino,
                ruta.HoraSalida,
                ruta.HoraRegreso,
                ruta.DesVehiculo,
                ruta.CuposIda,
                ruta.CuposVuelta,
                ruta.IdUsuario,
                ruta.IdVehiculo,
                ruta.DiasRuta
            );
        }

        public async Task<RutaListDto> CrearRutaAsync(CrearRutaDto dto)
        {
            var ruta = new Rutum
            {
                PuntoOrigen = dto.PuntoOrigen,
                PuntoDestino = dto.PuntoDestino,
                HoraSalida = dto.HoraSalida,
                HoraRegreso = dto.HoraRegreso,
                DesVehiculo = dto.DesVehiculo,
                CuposIda = dto.CuposIda,
                CuposVuelta = dto.CuposVuelta,
                IdUsuario = dto.IdUsuario,
                IdVehiculo = dto.IdVehiculo,
                DiasRuta = dto.DiasRuta
            };

            _context.Rutas.Add(ruta);
            await _context.SaveChangesAsync();

            return new RutaListDto(
                ruta.IdRuta,
                ruta.PuntoOrigen,
                ruta.PuntoDestino,
                ruta.HoraSalida,
                ruta.HoraRegreso,
                ruta.DesVehiculo,
                ruta.CuposIda,
                ruta.CuposVuelta,
                ruta.IdUsuario,
                ruta.IdVehiculo,
                ruta.DiasRuta
            );
        }

        public async Task<RutaListDto?> EditarRutaAsync(int id, EditarRutaDto dto)
        {
            var ruta = await _context.Rutas.FindAsync(id);
            if (ruta == null) return null;

            // Solo editar los campos permitidos
            ruta.HoraSalida = dto.HoraSalida;
            ruta.HoraRegreso = dto.HoraRegreso;
            ruta.DesVehiculo = dto.DesVehiculo;
            ruta.CuposIda = dto.CuposIda;
            ruta.CuposVuelta = dto.CuposVuelta;
            ruta.IdVehiculo = dto.IdVehiculo;
            ruta.DiasRuta = dto.DiasRuta ?? ruta.DiasRuta;

            await _context.SaveChangesAsync();

            return new RutaListDto(
                ruta.IdRuta,
                ruta.PuntoOrigen,
                ruta.PuntoDestino,
                ruta.HoraSalida,
                ruta.HoraRegreso,
                ruta.DesVehiculo,
                ruta.CuposIda,
                ruta.CuposVuelta,
                ruta.IdUsuario,
                ruta.IdVehiculo,
                ruta.DiasRuta
            );
        }

        public async Task<bool> EliminarRutaAsync(int id)
        {
            var ruta = await _context.Rutas.FindAsync(id);
            if (ruta == null) return false;

            _context.Rutas.Remove(ruta);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
