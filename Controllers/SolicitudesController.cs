using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using UniRumbo.Dtos;
using UniRumbo.Repositories;
using UniRumbo.Services.Interfaces;

namespace UniRumbo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SolicitudesController : ControllerBase
    {
        private readonly ISolicitudesService _service;

        public SolicitudesController(ISolicitudesService service)
        {
            _service = service;
        }

        // =========================================================
        // 🔹 CREAR SOLICITUDES
        // =========================================================

        // Crear solicitud de ruta
        [HttpPost("ruta")]
        public async Task<IActionResult> CrearSolicitudRuta([FromBody] SolicitudRutaDto dto)
        {
            var ok = await _service.CrearSolicitudRutaAsync(dto);
            return ok
                ? Ok("✅ Solicitud de ruta creada correctamente.")
                : BadRequest("❌ Error al crear la solicitud de ruta.");
        }

        // Crear solicitud de alojamiento
        [HttpPost("alojamiento")]
        public async Task<IActionResult> CrearSolicitudAlojamiento([FromBody] SolicitudAlojamientoDto dto)
        {
            var ok = await _service.CrearSolicitudAlojamientoAsync(dto);
            return ok
                ? Ok("✅ Solicitud de alojamiento creada correctamente.")
                : BadRequest("❌ Error al crear la solicitud de alojamiento.");
        }

        // =========================================================
        // 🔹 OBTENER SOLICITUDES
        // =========================================================

        // Obtener TODAS las solicitudes de ruta de un usuario
        [HttpGet("rutas/{idUsuario:int}")]
        public async Task<IActionResult> ObtenerSolicitudesRuta(int idUsuario)
        {
            var data = await _service.ObtenerSolicitudesRutaPendientesAsync(idUsuario);
            return Ok(data);
        }

        // Obtener TODAS las solicitudes de alojamiento de un usuario
        [HttpGet("alojamientos/{idUsuario:int}")]
        public async Task<IActionResult> ObtenerSolicitudesAlojamiento(int idUsuario)
        {
            var data = await _service.ObtenerSolicitudesAlojamientoPendientesAsync(idUsuario);
            return Ok(data);
        }

        // =========================================================
        // 🔹 OBTENER SOLICITUDES PENDIENTES (para el publicador)
        // =========================================================

        [HttpGet("rutas/pendientes/{idUsuario:int}")]
        public async Task<IActionResult> ObtenerSolicitudesRutaPendientes(int idUsuario)
        {
            var data = await _service.ObtenerSolicitudesRutaPendientesAsync(idUsuario);
            return Ok(data);
        }

        [HttpGet("alojamientos/pendientes/{idUsuario}")]
        public async Task<IActionResult> ObtenerSolicitudesAlojamientoPendientes(int idUsuario)
        {
            var data = await _service.ObtenerSolicitudesAlojamientoPendientesAsync(idUsuario);
            return Ok(data);
        }

        // =========================================================
        // 🔹 OBTENER SOLICITUDES HECHAS POR EL USUARIO (solicitante)
        // =========================================================

        // Obtener las solicitudes de RUTA hechas por el usuario (solicitante)
        [HttpGet("rutas/usuario/{idUsuario:int}")]
        public async Task<IActionResult> ObtenerMisSolicitudesRuta(int idUsuario)
        {
            var data = await _service.ObtenerMisSolicitudesRutaAsync(idUsuario);
            return Ok(data);
        }

        // Obtener las solicitudes de ALOJAMIENTO hechas por el usuario (solicitante)
        [HttpGet("alojamientos/usuario/{idUsuario:int}")]
        public async Task<IActionResult> ObtenerMisSolicitudesAlojamiento(int idUsuario)
        {
            var data = await _service.ObtenerMisSolicitudesAlojamientoAsync(idUsuario);
            return Ok(data);
        }

        // =========================================================
        // 🔹 ELIMINAR SOLICITUDES
        // =========================================================

        [HttpDelete("alojamiento/{id:int}")]
        public async Task<IActionResult> EliminarSolicitudAlojamiento(int id)
        {
            var ok = await _service.EliminarSolicitudAlojamientoAsync(id);
            return ok ? NoContent() : NotFound("❌ Solicitud de alojamiento no encontrada.");
        }

        [HttpDelete("ruta/{id:int}")]
        public async Task<IActionResult> EliminarSolicitudRuta(int id)
        {
            var ok = await _service.EliminarSolicitudRutaAsync(id);
            return ok ? NoContent() : NotFound("❌ Solicitud de ruta no encontrada.");
        }



        // =========================================================
        // 🔹 ACTUALIZAR ESTADOS
        // =========================================================

        [HttpPut("ruta/{idSolicitud}/estado/{idEstado}")]
        public async Task<IActionResult> ActualizarEstadoRuta(int idSolicitud, int idEstado)
        {
            var ok = await _service.ActualizarEstadoRutaAsync(idSolicitud, idEstado);
            return ok
                ? Ok("✅ Estado de solicitud de ruta actualizado.")
                : NotFound("❌ Solicitud de ruta no encontrada.");
        }

        [HttpPut("alojamiento/{idSolicitud}/estado/{idEstado}")]
        public async Task<IActionResult> ActualizarEstadoAlojamiento(int idSolicitud, int idEstado)
        {
            var ok = await _service.ActualizarEstadoAlojamientoAsync(idSolicitud, idEstado);
            return ok
                ? Ok("✅ Estado de solicitud de alojamiento actualizado.")
                : NotFound("❌ Solicitud de alojamiento no encontrada.");
        }
    }
}
