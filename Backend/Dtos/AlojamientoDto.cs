using System.ComponentModel.DataAnnotations;

public class AlojamientoCreateDto
{
    [Required(ErrorMessage = "La ubicación es obligatoria.")]
    public string Ubicacion { get; set; } = null!;

    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "Debe especificar el ID del usuario.")]
    public int Id_Usuario { get; set; }
}

public class AlojamientoDto
{
    public int IdAlojamiento { get; set; }
    public string Ubicacion { get; set; } = null!;
    public string? Descripcion { get; set; }
    public int Id_Usuario { get; set; }
    public string NombreUsuario { get; set; } = null!;
    public string GoogleMapsUrl { get; set; } = null!; // 🌍 Nuevo campo
}
