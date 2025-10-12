using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using UniRumbo.Backend.Dtos;
using UniRumbo.Backend.Repositories.Entities;
using UniRumbo.Repositories;

namespace UniRumbo.Backend.Services;

public class RutasService : Interfaces.IRutasService
{
  private readonly ApplicationDbContext _db;
  public RutasService(ApplicationDbContext db) { _db = db; }

  private static RutaListDto Proj(Ruta r, string tipoVeh) => new(
    r.IdRuta,
    $"Ruta - {r.PuntoDestino}", "Activa",
    r.PuntoOrigen, r.PuntoDestino, tipoVeh, r.CuposIda, r.HoraSalida, r.HoraRegreso);

  public async Task<IEnumerable<RutaListDto>> BuscarAsync(string? origen, string? destino, DateTime? fecha)
  {
    var q = _db.Rutas.Include(x => x.Vehiculo).AsQueryable();
    if (!string.IsNullOrWhiteSpace(origen))  q = q.Where(r => r.PuntoOrigen.Contains(origen));
    if (!string.IsNullOrWhiteSpace(destino)) q = q.Where(r => r.PuntoDestino.Contains(destino));
    if (fecha.HasValue) { var f = fecha.Value.Date; q = q.Where(r => r.HoraSalida.Date == f || (r.HoraRegreso.HasValue && r.HoraRegreso.Value.Date == f)); }

    var list = await q.OrderBy(r => r.HoraSalida).ToListAsync();
    return list.Select(r => Proj(r, r.Vehiculo?.TipoVehiculo == true ? "Moto" : "Carro"));
  }

  public async Task<RutaListDto?> ObtenerPorIdAsync(int id)
  {
    var r = await _db.Rutas.Include(x => x.Vehiculo).FirstOrDefaultAsync(x => x.IdRuta == id);
    return r is null ? null : Proj(r, r.Vehiculo?.TipoVehiculo == true ? "Moto" : "Carro");
  }

  public async Task<IEnumerable<RutaListDto>> ObtenerMiasAsync(int usuarioId)
  {
    var list = await _db.Rutas.Include(x=>x.Vehiculo).Where(r => r.IdUsuario == usuarioId)
      .OrderByDescending(r=>r.HoraSalida).ToListAsync();
    return list.Select(r => Proj(r, r.Vehiculo?.TipoVehiculo == true ? "Moto" : "Carro"));
  }

  public async Task<bool> EditarAsync(EditarRutaDto dto)
  {
    var r = await _db.Rutas.FirstOrDefaultAsync(x => x.IdRuta == dto.IdRuta);
    if (r is null) return false;
    r.PuntoOrigen = dto.PuntoOrigen;
    r.PuntoDestino = dto.PuntoDestino;
    r.HoraSalida = dto.HoraSalida;
    r.HoraRegreso = dto.HoraRegreso;
    r.CuposIda = dto.CuposIda;
    r.CuposVuelta = dto.CuposVuelta;
    r.IdUsuario = dto.IdUsuario;
    r.IdVehiculo = dto.IdVehiculo;
    r.OrigenLat = dto.OrigenLat; r.OrigenLon = dto.OrigenLon;
    r.DestinoLat = dto.DestinoLat; r.DestinoLon = dto.DestinoLon;
    await _db.SaveChangesAsync();
    return true;
  }

  // Usa OSRM público para obtener la geometría (GeoJSON)
  public async Task<object?> GeoAsync(int id, IHttpClientFactory http)
  {
    var r = await _db.Rutas.FirstOrDefaultAsync(x => x.IdRuta == id);
    if (r is null) return null;

    async Task<(double lat, double lon)?> GeocodeAsync(string q)
    {
      var cli = http.CreateClient();
      var url = $"https://nominatim.openstreetmap.org/search?format=json&limit=1&q={Uri.EscapeDataString(q)}";
      using var req = new HttpRequestMessage(HttpMethod.Get, url);
      req.Headers.TryAddWithoutValidation("User-Agent", "unirumbo-demo");
      var res = await cli.SendAsync(req);
      res.EnsureSuccessStatusCode();
      using var stream = await res.Content.ReadAsStreamAsync();
      using var doc = await JsonDocument.ParseAsync(stream);
      if (doc.RootElement.ValueKind != JsonValueKind.Array || doc.RootElement.GetArrayLength() == 0) return null;
      var el = doc.RootElement[0];
      return (el.GetProperty("lat").GetDouble(), el.GetProperty("lon").GetDouble());
    }

    double? oLat = r.OrigenLat, oLon = r.OrigenLon, dLat = r.DestinoLat, dLon = r.DestinoLon;
    if (oLat is null || oLon is null)
    {
      var gc = await GeocodeAsync(r.PuntoOrigen);
      if (gc is null) return null; oLat = gc?.lat; oLon = gc?.lon;
    }
    if (dLat is null || dLon is null)
    {
      var gc = await GeocodeAsync(r.PuntoDestino);
      if (gc is null) return null; dLat = gc?.lat; dLon = gc?.lon;
    }

    var osrmUrl = $"https://router.project-osrm.org/route/v1/driving/{oLon},{oLat};{dLon},{dLat}?overview=full&geometries=geojson";
    var httpCli = http.CreateClient();
    var osrm = await httpCli.GetStringAsync(osrmUrl);
    using var doc2 = JsonDocument.Parse(osrm);
    var geom = doc2.RootElement.GetProperty("routes")[0].GetProperty("geometry");

    var fc = new {
      type = "FeatureCollection",
      features = new object[] {
        new { type="Feature", geometry = geom, properties = new { kind="route" } },
        new { type="Feature", geometry = new { type="Point", coordinates = new [] { oLon, oLat } }, properties = new { kind="origin", name=r.PuntoOrigen } },
        new { type="Feature", geometry = new { type="Point", coordinates = new [] { dLon, dLat } }, properties = new { kind="destination", name=r.PuntoDestino } }
      }
    };
    return fc;
  }

  public async Task<int> CrearAsync(CrearRutaDto dto)
  {
    // Seed mínimo si el ambiente no tiene datos (Rol, Sede, Usuario, Vehículo)
    if (!await _db.Rol.AnyAsync())
    {
      _db.Rol.Add(new Rol { Nombre = "Demo", Descripcion = "Rol demo" });
      await _db.SaveChangesAsync();
    }
    if (!await _db.Sede.AnyAsync())
    {
      _db.Sede.Add(new Sede { Nombre = "Demo" });
      await _db.SaveChangesAsync();
    }
    if (!await _db.Usuario.AnyAsync())
    {
      var rolId = await _db.Rol.Select(r => r.IdRol).FirstAsync();
      var sedeId = await _db.Sede.Select(s => s.IdSede).FirstAsync();
      _db.Usuario.Add(new Usuario {
        Nombre = "Conductor",
        Apellido = "Demo",
        Numero = "0000000000",
        Correo = "demo@unirumbo.edu",
        Contrasena = "demo",
        IdRol = rolId,
        IdSede = sedeId
      });
      await _db.SaveChangesAsync();
    }
    if (!await _db.Vehiculos.AnyAsync())
    {
      _db.Vehiculos.Add(new Vehiculo { TipoVehiculo = false });
      await _db.SaveChangesAsync();
    }

    // Resolver usuario
    var userId = dto.IdUsuario;
    if (!await _db.Usuario.AnyAsync(u => u.IdUsuario == userId))
      userId = await _db.Usuario.Select(u => u.IdUsuario).FirstOrDefaultAsync();
    if (userId == 0) throw new Exception("No hay usuarios disponibles para asociar a la ruta.");

    // Resolver vehículo
    var vehiculoId = dto.IdVehiculo;
    if (!await _db.Vehiculos.AnyAsync(v => v.IdVehiculo == vehiculoId))
      vehiculoId = await _db.Vehiculos.Select(v => v.IdVehiculo).FirstOrDefaultAsync();
    if (vehiculoId == 0) throw new Exception("No hay vehículos disponibles para asociar a la ruta.");

    var e = new Ruta {
      PuntoOrigen = dto.PuntoOrigen,
      PuntoDestino = dto.PuntoDestino,
      HoraSalida = dto.HoraSalida,
      HoraRegreso = dto.HoraRegreso,
      CuposIda = dto.CuposIda,
      CuposVuelta = dto.CuposVuelta,
      IdUsuario = userId,
      IdVehiculo = vehiculoId,
      OrigenLat = dto.OrigenLat,
      OrigenLon = dto.OrigenLon,
      DestinoLat = dto.DestinoLat,
      DestinoLon = dto.DestinoLon
    };
    _db.Rutas.Add(e);
    await _db.SaveChangesAsync();
    return e.IdRuta;
  }
}
