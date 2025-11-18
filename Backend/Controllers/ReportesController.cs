using Microsoft.AspNetCore.Mvc;
using UniRumbo.Backend.Dtos;
using UniRumbo.Backend.Services;

namespace UniRumbo.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportesController : ControllerBase
    {
        private readonly IReportesService _reportesService;

        public ReportesController(IReportesService reportesService)
        {
            _reportesService = reportesService;
        }

        /// <summary>
        /// Genera un reporte PDF de las rutas más usadas
        /// </summary>
        [HttpPost("rutas-mas-usadas")]
        public IActionResult GenerarReporteRutasMasUsadas([FromBody] GenerarReporteDto dto)
        {
            try
            {
                var pdfBytes = _reportesService.GenerarReporteRutasMasUsadas(dto.UsuarioNombre);
                var fileName = $"Reporte_Rutas_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al generar el reporte", error = ex.Message });
            }
        }

        /// <summary>
        /// Genera un reporte PDF de los alojamientos disponibles
        /// </summary>
        [HttpPost("alojamientos")]
        public IActionResult GenerarReporteAlojamientos([FromBody] GenerarReporteDto dto)
        {
            try
            {
                var pdfBytes = _reportesService.GenerarReporteAlojamientos(dto.UsuarioNombre);
                var fileName = $"Reporte_Alojamientos_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al generar el reporte", error = ex.Message });
            }
        }
    }
}
