using Microsoft.EntityFrameworkCore;
using UniRumbo.Dtos;
using UniRumbo.Repositories;
using UniRumbo.Services.Interfaces;
using UniRumboBakend.Dtos;
//Servicio de Solicitudes para rutas y alojamientos, se encarga de la logica de negocio
namespace UniRumbo.Services
{
    public class SolicitudesService : ISolicitudesService
    {
        private readonly ApplicationDbContext _context;

        public SolicitudesService(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🔹 CREAR SOLICITUD DE RUTA
        public async Task<bool> CrearSolicitudRutaAsync(SolicitudRutaDto dto)
        {
            var solicitud = new SolicitudRutum
            {
                IdRuta = dto.IdRuta,
                IdUsuario = dto.IdUsuario,
                IdEstado = 1 // pendiente
            };

            _context.SolicitudRuta.Add(solicitud);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<object>> ObtenerSolicitudesRutaAsync(int idUsuario)
        {
            var solicitudes = await _context.SolicitudRuta
                .Include(s => s.Ruta)
                .Include(s => s.Estado)
                .Where(s => s.IdUsuario == idUsuario) // ✅ MIS solicitudes (solicitante)
                .Select(s => new
                {
                    s.IdSoliRuta,
                    Estado = s.Estado.EstadoNombre,
                    Ruta = new
                    {
                        s.Ruta.IdRuta,
                        s.Ruta.PuntoOrigen,
                        s.Ruta.PuntoDestino
                    }
                })
                .ToListAsync();

            return solicitudes;
        }


        public async Task<IEnumerable<object>> ObtenerSolicitudesAlojamientoAsync(int idUsuario)
        {
            var solicitudes = await _context.SolicitudAlojamiento
                .Include(s => s.Alojamiento)
                .Include(s => s.Estado)
                .Include(s => s.Usuario)
                .Where(s => s.Alojamiento.IdUsuario == idUsuario)
                .Select(s => new
                {
                    IdSolicitudAlojamiento = s.IdSoliAlojamiento, // ✅ renombrado igual que en tu DTO
                    Estado = s.Estado.EstadoNombre,
                    Alojamiento = new
                    {
                        s.Alojamiento.IdAlojamiento,
                        s.Alojamiento.Descripcion,
                        s.Alojamiento.Ubicacion
                    },
                    UsuarioSolicitante = new
                    {
                        s.Usuario.IdUsuario,
                        s.Usuario.Nombre
                    }
                })
                .ToListAsync();

            return solicitudes;
        }


        // 🔹 LISTAR SOLICITUDES DE RUTA PENDIENTES (del publicador)
        public async Task<IEnumerable<SolicitudRutum>> ObtenerSolicitudesRutaPendientesAsync(int idUsuario)
        {
            return await _context.SolicitudRuta
                .Include(s => s.Ruta)
                .Include(s => s.Usuario)
                .Where(s => s.Ruta.IdUsuario == idUsuario && s.IdEstado == 1)
                .ToListAsync();
        }

        // 🔹 ACTUALIZAR ESTADO DE RUTA
        public async Task<bool> ActualizarEstadoRutaAsync(int idSolicitud, int idEstado)
        {
            var solicitud = await _context.SolicitudRuta
                .Include(s => s.Ruta)
                .FirstOrDefaultAsync(s => s.IdSoliRuta == idSolicitud);

            if (solicitud == null) return false;

            solicitud.IdEstado = idEstado;

            if (idEstado == 2) // aceptado
                solicitud.Ruta.CuposIda = Math.Max(0, solicitud.Ruta.CuposIda - 1);

            await _context.SaveChangesAsync();
            return true;
        }


        // 🔹 CREAR SOLICITUD DE ALOJAMIENTO
        public async Task<bool> CrearSolicitudAlojamientoAsync(SolicitudAlojamientoDto dto)
        {
            var solicitud = new SolicitudAlojamiento
            {
                IdAlojamiento = dto.IdAlojamiento,
                IdUsuario = dto.IdUsuario,
                IdEstado = 1
            };

            _context.SolicitudAlojamiento.Add(solicitud);
            await _context.SaveChangesAsync();
            return true;
        }

        // =========================================================
        // 🔹 OBTENER SOLICITUDES HECHAS POR EL USUARIO (solicitante)
        // =========================================================

        public async Task<IEnumerable<object>> ObtenerMisSolicitudesRutaAsync(int idUsuario)
        {
            return await _context.SolicitudRuta
                .Include(s => s.Ruta)
                .Include(s => s.Estado)
                .Where(s => s.IdUsuario == idUsuario)
                .Select(s => new
                {
                    IdSolicitudRuta = s.IdSoliRuta,
                    Estado = s.Estado.EstadoNombre,
                    Ruta = new
                    {
                        s.Ruta.IdRuta,
                        s.Ruta.PuntoOrigen,
                        s.Ruta.PuntoDestino
                    }
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> ObtenerMisSolicitudesAlojamientoAsync(int idUsuario)
        {
            return await _context.SolicitudAlojamiento
                .Include(s => s.Alojamiento)
                .Include(s => s.Estado)
                .Include(s => s.Usuario)
                .Where(s => s.IdUsuario == idUsuario)
                .Select(s => new
                {
                    IdSolicitudAlojamiento = s.IdSoliAlojamiento,
                    Estado = s.Estado.EstadoNombre,
                    Alojamiento = new
                    {
                        s.Alojamiento.IdAlojamiento,
                        s.Alojamiento.Descripcion,
                        s.Alojamiento.Ubicacion
                    },
                    UsuarioSolicitante = new
                    {
                        s.Usuario.IdUsuario,
                        s.Usuario.Nombre
                    }
                })
                .ToListAsync();

            
        }
        // 🔹 ELIMINAR SOLICITUD DE ALOJAMIENTO
        public async Task<bool> EliminarSolicitudAlojamientoAsync(int id)
        {
            var solicitud = await _context.SolicitudAlojamiento.FindAsync(id);
            if (solicitud == null)
                return false;

            _context.SolicitudAlojamiento.Remove(solicitud);
            await _context.SaveChangesAsync();
            return true;
        }

        // 🔹 ELIMINAR SOLICITUD DE RUTA
        public async Task<bool> EliminarSolicitudRutaAsync(int id)
        {
            var solicitud = await _context.SolicitudRuta.FindAsync(id);
            if (solicitud == null)
                return false;

            _context.SolicitudRuta.Remove(solicitud);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task AddWithImagesAsync(AlojamientoWithImagesDto dto)
        {
            var alojamiento = new Alojamiento
            {
                Ubicacion = $"lat:{dto.Latitud},lon:{dto.Longitud}",
                Descripcion = dto.Descripcion,
                IdUsuario = dto.Id_Usuario,
                Imagenes = dto.ImagenesBase64.Select(base64 => new Imagenes
                {
                    Img = Convert.FromBase64String(base64)
                }).ToList()
            };

            await _context.AddAsync(alojamiento);
        }

        // 🔹 LISTAR SOLICITUDES DE ALOJAMIENTO PENDIENTES (del publicador)
        public async Task<IEnumerable<SolicitudAlojamiento>> ObtenerSolicitudesAlojamientoPendientesAsync(int idUsuario)
        {
            return await _context.SolicitudAlojamiento
                .Include(s => s.Alojamiento)
                .Include(s => s.Usuario)
                .Where(s => s.Alojamiento.IdUsuario == idUsuario && s.IdEstado == 1)
                .ToListAsync();
        }

        // 🔹 ACTUALIZAR ESTADO DE ALOJAMIENTO
        public async Task<bool> ActualizarEstadoAlojamientoAsync(int idSolicitud, int idEstado)
        {
            var solicitud = await _context.SolicitudAlojamiento
                .Include(s => s.Alojamiento)
                .FirstOrDefaultAsync(s => s.IdSoliAlojamiento == idSolicitud);

            if (solicitud == null) return false;

            solicitud.IdEstado = idEstado;

            if (idEstado == 2) // aceptado
                solicitud.Alojamiento.IdUsuario = solicitud.IdUsuario; // opcional: marcar como asignado

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
