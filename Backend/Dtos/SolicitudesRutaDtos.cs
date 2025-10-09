namespace UniRumbo.Backend.Dtos;

public class CrearSolicitudRutaDto {
  public int IdUsuario { get; set; }
  public int IdRuta { get; set; }
  public int IdEstado { get; set; } // 1 = Pendiente
}
public record SolicitudListDto(int Id, int IdRuta, string Estado, DateTime Creado);
