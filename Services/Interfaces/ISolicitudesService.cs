using UniRumbo.Dtos;
using UniRumbo.Repositories;

public interface ISolicitudesService
{
    Task<bool> CrearSolicitudRutaAsync(SolicitudRutaDto dto);
    Task<IEnumerable<object>> ObtenerSolicitudesRutaAsync(int idUsuario);                // 👈 NUEVO
    Task<IEnumerable<SolicitudRutum>> ObtenerSolicitudesRutaPendientesAsync(int idUsuario);

    Task<bool> CrearSolicitudAlojamientoAsync(SolicitudAlojamientoDto dto);
    Task<IEnumerable<object>> ObtenerSolicitudesAlojamientoAsync(int idUsuario);
    Task<IEnumerable<SolicitudAlojamiento>> ObtenerSolicitudesAlojamientoPendientesAsync(int idUsuario);

    Task<bool> EliminarSolicitudAlojamientoAsync(int id);
    Task<bool> EliminarSolicitudRutaAsync(int id);

    Task<IEnumerable<object>> ObtenerMisSolicitudesRutaAsync(int idUsuario);
    Task<IEnumerable<object>> ObtenerMisSolicitudesAlojamientoAsync(int idUsuario);

    Task<bool> ActualizarEstadoRutaAsync(int idSolicitud, int idEstado);
    Task<bool> ActualizarEstadoAlojamientoAsync(int idSolicitud, int idEstado);
}
