using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UniRumbo.Repositories;

public partial class Usuario
{
    [Key]
    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = null!;

    [Column("apellido")]
    public string Apellido { get; set; } = null!;

    [Column("numero")]
    public required string Numero { get; set; }

    [Column("correo")]
    public string Correo { get; set; } = null!;

    [Column("contrasena")]
    public string Contrasena { get; set; } = null!;

    [JsonPropertyName("id_rol")]
    public int IdRol { get; set; }

    [JsonPropertyName("id_sede")]
    public int IdSede { get; set; }
    [Column("reset_token")]
    public string? ResetToken { get; set; }
    [Column("reset_token_expires_at")]
    public DateTime? ResetTokenExpires { get; set; }

    public virtual ICollection<Alojamiento> Alojamientos { get; set; } = new List<Alojamiento>();

    public virtual Rol IdRolNavigation { get; set; } = null!;

    public virtual Sede IdSedeNavigation { get; set; } = null!;

    public virtual ICollection<Rutum> Ruta { get; set; } = new List<Rutum>();

    public virtual ICollection<SolicitudAlojamiento> SolicitudAlojamientos { get; set; } = new List<SolicitudAlojamiento>();

    public virtual ICollection<SolicitudRutum> SolicitudRuta { get; set; } = new List<SolicitudRutum>();
}
