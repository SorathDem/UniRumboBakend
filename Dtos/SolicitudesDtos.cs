namespace UniRumbo.Api.Models.Dtos;

public class CrearSolicitudRutaDto
{
    public int IdUsuario { get; set; }
    public int IdRuta { get; set; }
    public int IdEstado { get; set; } // catálogo Estado
}

public record SolicitudListDto(int Id, int IdRuta, string Estado, DateTime Creado);