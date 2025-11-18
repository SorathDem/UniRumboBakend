using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using UniRumbo.Backend.Dtos;

namespace UniRumbo.Backend.Services
{
    public interface IReportesService
    {
        byte[] GenerarReporteRutasMasUsadas(string usuarioNombre);
        byte[] GenerarReporteAlojamientos(string usuarioNombre);
    }

    public class ReportesService : IReportesService
    {
        public ReportesService()
        {
            // Configuración de licencia comunitaria de QuestPDF
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public byte[] GenerarReporteRutasMasUsadas(string usuarioNombre)
        {
            var datos = ObtenerDatosFictiosRutas();
            var fechaGeneracion = DateTime.Now;

            var documento = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                    // ENCABEZADO
                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Text("UNIRUMBO")
                                .FontSize(28).Bold().FontColor("#1E3A8A");
                            
                            col.Item().PaddingTop(5).AlignCenter().Text("Reporte de Rutas Más Usadas")
                                .FontSize(16).SemiBold().FontColor("#3B82F6");
                            
                            col.Item().PaddingTop(8).BorderBottom(2).BorderColor("#3B82F6");
                        });
                    });

                    // CONTENIDO
                    page.Content().PaddingTop(10).Column(col =>
                    {
                        // Información del reporte
                        col.Item().Background("#EFF6FF").Padding(10).Row(row =>
                        {
                            row.RelativeItem().Column(infoCol =>
                            {
                                infoCol.Item().Text(txt =>
                                {
                                    txt.Span("Fecha de generación: ").SemiBold();
                                    txt.Span(fechaGeneracion.ToString("dd/MM/yyyy HH:mm:ss"));
                                });
                                infoCol.Item().PaddingTop(5).Text(txt =>
                                {
                                    txt.Span("Generado por: ").SemiBold();
                                    txt.Span(usuarioNombre).FontColor("#1E40AF");
                                });
                            });
                        });

                        // Espacio
                        col.Item().PaddingTop(15);

                        // Título de la tabla
                        col.Item().Text("Rutas con Mayor Uso - Estadísticas Generales")
                            .FontSize(14).SemiBold().FontColor("#1E3A8A");

                        col.Item().PaddingTop(8);

                        // Tabla de datos
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40);  // ID
                                columns.RelativeColumn(2);    // Origen
                                columns.RelativeColumn(2);    // Destino
                                columns.ConstantColumn(60);   // Usos
                                columns.ConstantColumn(70);   // Usuarios
                                columns.RelativeColumn(1.5f); // Día
                                columns.ConstantColumn(70);   // Distancia
                            });

                            // Encabezado de tabla
                            table.Header(header =>
                            {
                                header.Cell().Background("#1E3A8A").Padding(5)
                                    .Text("ID").FontColor(Colors.White).SemiBold().FontSize(10);
                                header.Cell().Background("#1E3A8A").Padding(5)
                                    .Text("Origen").FontColor(Colors.White).SemiBold().FontSize(10);
                                header.Cell().Background("#1E3A8A").Padding(5)
                                    .Text("Destino").FontColor(Colors.White).SemiBold().FontSize(10);
                                header.Cell().Background("#1E3A8A").Padding(5)
                                    .Text("Usos").FontColor(Colors.White).SemiBold().FontSize(10);
                                header.Cell().Background("#1E3A8A").Padding(5)
                                    .Text("Usuarios").FontColor(Colors.White).SemiBold().FontSize(10);
                                header.Cell().Background("#1E3A8A").Padding(5)
                                    .Text("Día +Usado").FontColor(Colors.White).SemiBold().FontSize(10);
                                header.Cell().Background("#1E3A8A").Padding(5)
                                    .Text("Distancia").FontColor(Colors.White).SemiBold().FontSize(10);
                            });

                            // Filas de datos
                            var colorAlternativo = true;
                            foreach (var ruta in datos)
                            {
                                var bgColor = colorAlternativo ? "#F1F5F9" : "#FFFFFF";
                                colorAlternativo = !colorAlternativo;

                                table.Cell().Background(bgColor).Padding(5).Text(ruta.Id.ToString()).FontSize(9);
                                table.Cell().Background(bgColor).Padding(5).Text(ruta.Origen).FontSize(9);
                                table.Cell().Background(bgColor).Padding(5).Text(ruta.Destino).FontSize(9);
                                table.Cell().Background(bgColor).Padding(5).Text(ruta.CantidadUsos.ToString()).FontSize(9);
                                table.Cell().Background(bgColor).Padding(5).Text(ruta.CantidadUsuarios.ToString()).FontSize(9);
                                table.Cell().Background(bgColor).Padding(5).Text(ruta.DiaMasUsado).FontSize(9);
                                table.Cell().Background(bgColor).Padding(5).Text($"{ruta.DistanciaKm:F1} km").FontSize(9);
                            }
                        });

                        // Resumen estadístico
                        col.Item().PaddingTop(15).Background("#DBEAFE").Padding(10).Row(row =>
                        {
                            row.RelativeItem().Column(statsCol =>
                            {
                                statsCol.Item().Text("📊 Resumen Estadístico").FontSize(12).SemiBold().FontColor("#1E40AF");
                                statsCol.Item().PaddingTop(5).Text($"Total de rutas: {datos.Count}");
                                statsCol.Item().Text($"Total de usos: {datos.Sum(r => r.CantidadUsos)}");
                                statsCol.Item().Text($"Promedio de usuarios por ruta: {datos.Average(r => r.CantidadUsuarios):F1}");
                                statsCol.Item().Text($"Distancia total: {datos.Sum(r => r.DistanciaKm):F1} km");
                            });
                        });
                    });

                    // PIE DE PÁGINA
                    page.Footer().AlignCenter().Column(col =>
                    {
                        col.Item().BorderTop(1).BorderColor("#CBD5E1");
                        col.Item().PaddingTop(5).Text(txt =>
                        {
                            txt.Span("UniRumbo © 2025 - Sistema de Gestión de Rutas Universitarias").FontSize(9).FontColor("#64748B");
                        });
                        col.Item().Text(txt =>
                        {
                            txt.CurrentPageNumber().FontSize(8);
                            txt.Span(" / ").FontSize(8);
                            txt.TotalPages().FontSize(8);
                        });
                    });
                });
            });

            return documento.GeneratePdf();
        }

        public byte[] GenerarReporteAlojamientos(string usuarioNombre)
        {
            var datos = ObtenerDatosFictiosAlojamientos();
            var fechaGeneracion = DateTime.Now;

            var documento = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                    // ENCABEZADO
                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Text("UNIRUMBO")
                                .FontSize(28).Bold().FontColor("#047857");
                            
                            col.Item().PaddingTop(5).AlignCenter().Text("Reporte de Alojamientos Disponibles")
                                .FontSize(16).SemiBold().FontColor("#10B981");
                            
                            col.Item().PaddingTop(8).BorderBottom(2).BorderColor("#10B981");
                        });
                    });

                    // CONTENIDO
                    page.Content().PaddingTop(10).Column(col =>
                    {
                        // Información del reporte
                        col.Item().Background("#ECFDF5").Padding(10).Row(row =>
                        {
                            row.RelativeItem().Column(infoCol =>
                            {
                                infoCol.Item().Text(txt =>
                                {
                                    txt.Span("Fecha de generación: ").SemiBold();
                                    txt.Span(fechaGeneracion.ToString("dd/MM/yyyy HH:mm:ss"));
                                });
                                infoCol.Item().PaddingTop(5).Text(txt =>
                                {
                                    txt.Span("Generado por: ").SemiBold();
                                    txt.Span(usuarioNombre).FontColor("#065F46");
                                });
                            });
                        });

                        col.Item().PaddingTop(15);

                        // Título de la tabla
                        col.Item().Text("Estado de Ocupación de Alojamientos por Sede")
                            .FontSize(14).SemiBold().FontColor("#047857");

                        col.Item().PaddingTop(8);

                        // Tabla de datos
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(35);   // ID
                                columns.RelativeColumn(2.5f); // Nombre
                                columns.RelativeColumn(2);    // Dirección
                                columns.RelativeColumn(1.2f); // Sede
                                columns.ConstantColumn(65);   // Capacidad
                                columns.ConstantColumn(65);   // Ocupados
                                columns.ConstantColumn(70);   // %
                                columns.RelativeColumn(1);    // Estado
                            });

                            // Encabezado de tabla
                            table.Header(header =>
                            {
                                header.Cell().Background("#047857").Padding(5)
                                    .Text("ID").FontColor(Colors.White).SemiBold().FontSize(9);
                                header.Cell().Background("#047857").Padding(5)
                                    .Text("Nombre").FontColor(Colors.White).SemiBold().FontSize(9);
                                header.Cell().Background("#047857").Padding(5)
                                    .Text("Dirección").FontColor(Colors.White).SemiBold().FontSize(9);
                                header.Cell().Background("#047857").Padding(5)
                                    .Text("Sede").FontColor(Colors.White).SemiBold().FontSize(9);
                                header.Cell().Background("#047857").Padding(5)
                                    .Text("Capacidad").FontColor(Colors.White).SemiBold().FontSize(9);
                                header.Cell().Background("#047857").Padding(5)
                                    .Text("Ocupados").FontColor(Colors.White).SemiBold().FontSize(9);
                                header.Cell().Background("#047857").Padding(5)
                                    .Text("Ocupación").FontColor(Colors.White).SemiBold().FontSize(9);
                                header.Cell().Background("#047857").Padding(5)
                                    .Text("Estado").FontColor(Colors.White).SemiBold().FontSize(9);
                            });

                            // Filas de datos
                            var colorAlternativo = true;
                            foreach (var aloj in datos)
                            {
                                var bgColor = colorAlternativo ? "#F0FDF4" : "#FFFFFF";
                                colorAlternativo = !colorAlternativo;

                                // Color del estado
                                var estadoColor = aloj.EstadoActual == "Disponible" ? "#10B981" : 
                                                 aloj.EstadoActual == "Lleno" ? "#EF4444" : "#F59E0B";

                                table.Cell().Background(bgColor).Padding(5).Text(aloj.Id.ToString()).FontSize(9);
                                table.Cell().Background(bgColor).Padding(5).Text(aloj.Nombre).FontSize(9);
                                table.Cell().Background(bgColor).Padding(5).Text(aloj.Direccion).FontSize(9);
                                table.Cell().Background(bgColor).Padding(5).Text(aloj.Sede).FontSize(9);
                                table.Cell().Background(bgColor).Padding(5).Text(aloj.CapacidadTotal.ToString()).FontSize(9);
                                table.Cell().Background(bgColor).Padding(5).Text(aloj.EspaciosOcupados.ToString()).FontSize(9);
                                table.Cell().Background(bgColor).Padding(5).Text($"{aloj.PorcentajeOcupacion:F0}%").FontSize(9);
                                table.Cell().Background(bgColor).Padding(5).Text(aloj.EstadoActual)
                                    .FontSize(9).FontColor(estadoColor).SemiBold();
                            }
                        });

                        // Resumen estadístico
                        col.Item().PaddingTop(15).Background("#D1FAE5").Padding(10).Row(row =>
                        {
                            row.RelativeItem().Column(statsCol =>
                            {
                                var totalCapacidad = datos.Sum(a => a.CapacidadTotal);
                                var totalOcupados = datos.Sum(a => a.EspaciosOcupados);
                                var ocupacionGeneral = (totalOcupados * 100.0 / totalCapacidad);

                                statsCol.Item().Text("📊 Resumen de Ocupación").FontSize(12).SemiBold().FontColor("#065F46");
                                statsCol.Item().PaddingTop(5).Text($"Total de alojamientos: {datos.Count}");
                                statsCol.Item().Text($"Capacidad total: {totalCapacidad} espacios");
                                statsCol.Item().Text($"Espacios ocupados: {totalOcupados} espacios");
                                statsCol.Item().Text($"Espacios disponibles: {totalCapacidad - totalOcupados} espacios");
                                statsCol.Item().Text($"Ocupación general: {ocupacionGeneral:F1}%").SemiBold();
                            });
                        });
                    });

                    // PIE DE PÁGINA
                    page.Footer().AlignCenter().Column(col =>
                    {
                        col.Item().BorderTop(1).BorderColor("#A7F3D0");
                        col.Item().PaddingTop(5).Text(txt =>
                        {
                            txt.Span("UniRumbo © 2025 - Sistema de Gestión de Alojamiento Universitario").FontSize(9).FontColor("#64748B");
                        });
                        col.Item().Text(txt =>
                        {
                            txt.CurrentPageNumber().FontSize(8);
                            txt.Span(" / ").FontSize(8);
                            txt.TotalPages().FontSize(8);
                        });
                    });
                });
            });

            return documento.GeneratePdf();
        }

        // Datos ficticios para rutas más usadas
        private List<RutaMasUsadaDto> ObtenerDatosFictiosRutas()
        {
            return new List<RutaMasUsadaDto>
            {
                new RutaMasUsadaDto { Id = 1, Origen = "Sede Norte", Destino = "Sede Centro", CantidadUsos = 145, CantidadUsuarios = 78, DiaMasUsado = "Lunes", DistanciaKm = 8.5m },
                new RutaMasUsadaDto { Id = 2, Origen = "Sede Sur", Destino = "Sede Norte", CantidadUsos = 132, CantidadUsuarios = 65, DiaMasUsado = "Martes", DistanciaKm = 12.3m },
                new RutaMasUsadaDto { Id = 3, Origen = "Sede Centro", Destino = "Sede Oriental", CantidadUsos = 98, CantidadUsuarios = 52, DiaMasUsado = "Miércoles", DistanciaKm = 6.7m },
                new RutaMasUsadaDto { Id = 4, Origen = "Sede Occidental", Destino = "Sede Sur", CantidadUsos = 87, CantidadUsuarios = 45, DiaMasUsado = "Jueves", DistanciaKm = 10.2m },
                new RutaMasUsadaDto { Id = 5, Origen = "Sede Oriental", Destino = "Sede Centro", CantidadUsos = 76, CantidadUsuarios = 41, DiaMasUsado = "Viernes", DistanciaKm = 5.8m },
                new RutaMasUsadaDto { Id = 6, Origen = "Sede Norte", Destino = "Sede Sur", CantidadUsos = 65, CantidadUsuarios = 38, DiaMasUsado = "Lunes", DistanciaKm = 15.4m },
                new RutaMasUsadaDto { Id = 7, Origen = "Sede Centro", Destino = "Sede Occidental", CantidadUsos = 54, CantidadUsuarios = 32, DiaMasUsado = "Martes", DistanciaKm = 9.1m },
                new RutaMasUsadaDto { Id = 8, Origen = "Sede Sur", Destino = "Sede Oriental", CantidadUsos = 43, CantidadUsuarios = 28, DiaMasUsado = "Miércoles", DistanciaKm = 11.6m },
            };
        }

        // Datos ficticios para alojamientos
        private List<AlojamientoReporteDto> ObtenerDatosFictiosAlojamientos()
        {
            return new List<AlojamientoReporteDto>
            {
                new AlojamientoReporteDto { Id = 1, Nombre = "Residencia Universitaria El Bosque", Direccion = "Calle 45 #23-67", Sede = "Norte", CapacidadTotal = 50, EspaciosOcupados = 42, PorcentajeOcupacion = 84, EstadoActual = "Casi Lleno" },
                new AlojamientoReporteDto { Id = 2, Nombre = "Apartamentos Los Andes", Direccion = "Carrera 15 #78-90", Sede = "Centro", CapacidadTotal = 30, EspaciosOcupados = 18, PorcentajeOcupacion = 60, EstadoActual = "Disponible" },
                new AlojamientoReporteDto { Id = 3, Nombre = "Residencia Femenina La Colina", Direccion = "Avenida 32 #12-34", Sede = "Sur", CapacidadTotal = 40, EspaciosOcupados = 40, PorcentajeOcupacion = 100, EstadoActual = "Lleno" },
                new AlojamientoReporteDto { Id = 4, Nombre = "Casa Estudiantil El Parque", Direccion = "Calle 67 #45-12", Sede = "Oriental", CapacidadTotal = 25, EspaciosOcupados = 15, PorcentajeOcupacion = 60, EstadoActual = "Disponible" },
                new AlojamientoReporteDto { Id = 5, Nombre = "Residencia Masculina Los Pinos", Direccion = "Carrera 28 #56-89", Sede = "Occidental", CapacidadTotal = 35, EspaciosOcupados = 28, PorcentajeOcupacion = 80, EstadoActual = "Casi Lleno" },
                new AlojamientoReporteDto { Id = 6, Nombre = "Apartaestudios La Paz", Direccion = "Calle 89 #34-56", Sede = "Norte", CapacidadTotal = 20, EspaciosOcupados = 12, PorcentajeOcupacion = 60, EstadoActual = "Disponible" },
                new AlojamientoReporteDto { Id = 7, Nombre = "Residencia Mixta El Futuro", Direccion = "Avenida 12 #67-23", Sede = "Centro", CapacidadTotal = 45, EspaciosOcupados = 38, PorcentajeOcupacion = 84, EstadoActual = "Casi Lleno" },
                new AlojamientoReporteDto { Id = 8, Nombre = "Casa Universitaria La Esperanza", Direccion = "Carrera 56 #89-01", Sede = "Sur", CapacidadTotal = 28, EspaciosOcupados = 20, PorcentajeOcupacion = 71, EstadoActual = "Disponible" },
            };
        }
    }
}
