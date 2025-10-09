using UniRumbo.Repositories;

namespace UniRumbo.Backend.Repositories.Entities;

public class SolicitudRuta
{
    public int IdSoliRuta { get; set; }
    public int IdEstado { get; set; }     // catálogo Estado (ej. 1 Pendiente, 2 Aceptada…)
    public int IdUsuario { get; set; }    // solicitante (cliente)
    public int IdRuta { get; set; }

    public Estado? Estado { get; set; }
    public Usuario? Usuario { get; set; }
    public Ruta? Ruta { get; set; }
}
