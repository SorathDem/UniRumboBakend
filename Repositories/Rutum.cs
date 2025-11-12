using System.ComponentModel.DataAnnotations.Schema;
using UniRumbo.Repositories;
// Asegúrate de tener el using para el atributo Key si IdRuta es tu PK
// Si estás usando EF Core, no necesitas el using UniRumbo.Repositories

[Table("Ruta")]
public class Rutum
{
    // Propiedad de Clave Primaria (PK)
    [Column("id_ruta")]
    public int IdRuta { get; set; }

    // --- Otras propiedades de la ruta (se mantienen igual) ---
    [Column("punto_origen")]
    public string PuntoOrigen { get; set; } = null!;

    [Column("punto_destino")]
    public string PuntoDestino { get; set; } = null!;

    [Column("hora_salida")]
    public DateTime HoraSalida { get; set; }

    [Column("hora_regreso")]
    public DateTime HoraRegreso { get; set; }
    [Column("descripcion_vechivulo")]
    public string? DesVehiculo { get; set; }

    [Column("cupos_ida")]
    public int CuposIda { get; set; }

    [Column("cupos_vuelta")]
    public int CuposVuelta { get; set; }

    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("id_vehiculo")]
    public int IdVehiculo { get; set; }

    [Column("dias_ruta")]
    public string? DiasRuta { get; set; }

    // --- Propiedades de Navegación (Relaciones) ---

    // 1. Relación con Usuario:
    // [ForeignKey] debe apuntar al nombre de la *propiedad CLR* (IdUsuario).
    public Usuario Usuario { get; set; } = null!;
    public Vehiculo Vehiculo { get; set; } = null!;
}