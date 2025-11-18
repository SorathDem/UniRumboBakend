namespace UniRumbo.Api.Models.Dtos;
//DTOs para Solicitudes de Rutas y Alojamiento, se encargan de transportar datos entre el cliente y el servidor
public class CrearSolicitudRutaDto
{
    public int IdUsuario { get; set; }
    public int IdRuta { get; set; }
    public int IdEstado { get; set; } // catálogo Estado
}

public record SolicitudListDto(int Id, int IdRuta, string Estado, DateTime Creado);