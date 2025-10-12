using Microsoft.EntityFrameworkCore;
using UniRumbo.Backend.Repositories.Entities;

namespace UniRumbo.Repositories;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alojamiento> Alojamiento { get; set; }
    public virtual DbSet<Estado> Estados { get; set; }
    public virtual DbSet<Rol> Rol { get; set; }
    public virtual DbSet<Ruta> Rutas { get; set; }
    public virtual DbSet<Sede> Sede { get; set; }
    public virtual DbSet<SolicitudAlojamiento> SolicitudAlojamientos { get; set; }
    public virtual DbSet<SolicitudRuta> SolicitudesRuta { get; set; }
    public virtual DbSet<Usuario> Usuario { get; set; }
    public virtual DbSet<Vehiculo> Vehiculos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
            // ============================
            // Tabla Ruta  (usa tu entidad Rutum)
            // ============================
            modelBuilder.Entity<Ruta>(entity =>
            {
            entity.HasKey(e => e.IdRuta).HasName("PK__Ruta__id_ruta");
            entity.ToTable("Ruta");

            entity.Property(e => e.IdRuta).HasColumnName("id_ruta");
            entity.Property(e => e.PuntoOrigen)
            .HasMaxLength(150).IsUnicode(false)
            .HasColumnName("punto_origen");
            entity.Property(e => e.PuntoDestino)
            .HasMaxLength(150).IsUnicode(false)
            .HasColumnName("punto_destino");
            entity.Property(e => e.HoraSalida).HasColumnName("hora_salida");
            entity.Property(e => e.HoraRegreso).HasColumnName("hora_regreso");
            entity.Property(e => e.CuposIda).HasColumnName("cupos_ida");
            entity.Property(e => e.CuposVuelta).HasColumnName("cupos_vuelta");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.IdVehiculo).HasColumnName("id_vehiculo");

            // Si la tabla no tiene columnas de coordenadas, ignora estas propiedades para evitar errores.
            entity.Ignore(e => e.OrigenLat);
            entity.Ignore(e => e.OrigenLon);
            entity.Ignore(e => e.DestinoLat);
            entity.Ignore(e => e.DestinoLon);
        });

            // ============================
            // Tabla Solicitud_ruta (usa tu entidad SolicitudRutum)
            // ============================
            modelBuilder.Entity<SolicitudRuta>(entity =>
            {
                entity.HasKey(e => e.IdSoliRuta).HasName("PK__Solicitud_ruta__id_SoliRuta");
                entity.ToTable("Solicitud_ruta");

                entity.Property(e => e.IdSoliRuta).HasColumnName("id_SoliRuta");
                entity.Property(e => e.IdEstado).HasColumnName("id_estado");
                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
                entity.Property(e => e.IdRuta).HasColumnName("id_ruta");
            });

        // ============================
        // Tabla Alojamiento
        // ============================
        modelBuilder.Entity<Alojamiento>(entity =>
        {
            entity.HasKey(e => e.IdAlojamiento).HasName("PK__Alojamie__2908794AF1C66E30");

            entity.ToTable("Alojamiento");

            entity.Property(e => e.IdAlojamiento).HasColumnName("id_alojamiento");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Id_Usuario).HasColumnName("id_usuario");
            entity.Property(e => e.Ubicacion)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ubicacion");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Alojamientos)
                .HasForeignKey(d => d.Id_Usuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Alojamien__id_us__34C8D9D1");
        });

        // ============================
        // Tabla Usuario
        // ============================
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__4E3E04AD12345678");

            entity.ToTable("Usuario");

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Numero)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("numero");
            entity.Property(e => e.Correo)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("contrasena");

            // 👇 Foreign keys
            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.IdSede).HasColumnName("id_sede");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuario__id_rol__2A4B4B5E");

            entity.HasOne(d => d.IdSedeNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdSede)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuario__id_sede__29572725");
        });

        // ============================
        // Tabla Rol
        // ============================
        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Rol__3214EC0723456789");

            entity.ToTable("Rol");

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        // ============================
        // Tabla Sede
        // ============================
        modelBuilder.Entity<Sede>(entity =>
        {
            entity.HasKey(e => e.IdSede).HasName("PK__Sede__3214EC07ABCDEF12");

            entity.ToTable("Sede");

            entity.Property(e => e.IdSede).HasColumnName("id_sede");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
