using System.ComponentModel.DataAnnotations;

namespace UniRumbo.Dtos
{
    public class AlojamientoDto
    {
        public int IdAlojamiento { get; set; }
        public string Ubicacion { get; set; } = null!;
        public string? Descripcion { get; set; }
        public int Id_Usuario { get; set; }
    }

    // ✅ DTO para crear
    public class AlojamientoCreateDto
    {
        [Required(ErrorMessage = "La ubicación es obligatoria.")]
        public string Ubicacion { get; set; } = null!;

        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "Debe especificar el ID del usuario.")]
        public int Id_Usuario { get; set; }
    }

    // ✅ DTO para actualizar
    public class AlojamientoUpdateDto
    {
        [Required(ErrorMessage = "La ubicación es obligatoria.")]
        public string Ubicacion { get; set; } = null!;

        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "Debe especificar el ID del usuario.")]
        public int Id_Usuario { get; set; }
    }
}
