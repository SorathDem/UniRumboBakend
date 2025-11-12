using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using UniRumbo.Repositories;

namespace UniRumbo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReportesController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpGet("usuarios-pdf")]
        public async Task<IActionResult> GenerarUsuariosPdf([FromQuery] string usuarioGenerador = "Administrador")
        {
            
            QuestPDF.Settings.License = LicenseType.Community;

            var usuarios = await _context.Usuario.ToListAsync();

            if (usuarios == null || usuarios.Count == 0)
                return NotFound("No hay usuarios registrados para generar el reporte.");

            
            var fechaHoraGeneracion = DateTime.Now;

            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);
                    page.Size(PageSizes.A4);
                    page.DefaultTextStyle(x => x.FontSize(11));

                   
                    page.Header()
                        .Row(row =>
                        {
                            row.RelativeColumn()
                               .Column(col =>
                               {
                                   col.Item().Text("UniRumbo")
                                       .FontSize(22)
                                       .Bold()
                                       .FontColor(Colors.Blue.Medium);

                                   col.Item().Text("Reporte de Usuarios")
                                       .FontSize(14)
                                       .FontColor(Colors.Grey.Darken2);
                               });

                            row.ConstantColumn(80)
                               .AlignRight()
                               .Image(Placeholders.Image(80, 40));
                        });

                    
                    page.Content()
                        .Column(col =>
                        {
                            col.Item().PaddingBottom(10)
                                .Text($"Generado por: {usuarioGenerador}")
                                .FontSize(11)
                                .Italic()
                                .FontColor(Colors.Grey.Darken2);

                            col.Item().PaddingBottom(10)
                                .Text($"Fecha y hora: {fechaHoraGeneracion:dd/MM/yyyy HH:mm}")
                                .FontSize(11)
                                .FontColor(Colors.Grey.Darken2);

                            col.Item().PaddingVertical(10);

                            
                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(40);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(2);
                                });

                               
                                table.Header(header =>
                                {
                                    header.Cell().Background(Colors.Blue.Medium).Padding(5).Text("ID").FontColor(Colors.White).Bold();
                                    header.Cell().Background(Colors.Blue.Medium).Padding(5).Text("Nombre").FontColor(Colors.White).Bold();
                                    header.Cell().Background(Colors.Blue.Medium).Padding(5).Text("Apellido").FontColor(Colors.White).Bold();
                                    header.Cell().Background(Colors.Blue.Medium).Padding(5).Text("Correo").FontColor(Colors.White).Bold();
                                    header.Cell().Background(Colors.Blue.Medium).Padding(5).Text("Número").FontColor(Colors.White).Bold();
                                });

                                // 🔹 Filas con color alternado
                                int index = 0;
                                foreach (var usuario in usuarios)
                                {
                                    var backgroundColor = index % 2 == 0 ? Colors.Grey.Lighten4 : Colors.White;
                                    index++;

                                    table.Cell().Background(backgroundColor).Padding(5).Text(usuario.IdUsuario.ToString());
                                    table.Cell().Background(backgroundColor).Padding(5).Text(usuario.Nombre ?? "—");
                                    table.Cell().Background(backgroundColor).Padding(5).Text(usuario.Apellido ?? "—");
                                    table.Cell().Background(backgroundColor).Padding(5).Text(usuario.Correo ?? "—");
                                    table.Cell().Background(backgroundColor).Padding(5).Text(usuario.Numero ?? "—");
                                }
                            });
                        });

                   
                    page.Footer()
                        .AlignCenter()
                        .Text($"Generado por {usuarioGenerador} • {fechaHoraGeneracion:dd/MM/yyyy HH:mm}")
                        .FontSize(10)
                        .FontColor(Colors.Grey.Darken1)
                        .Italic();
                });
            }).GeneratePdf();

            return File(pdfBytes, "application/pdf", $"Reporte_Usuarios_{DateTime.Now:yyyyMMdd_HHmm}.pdf");
        }
    }
}
