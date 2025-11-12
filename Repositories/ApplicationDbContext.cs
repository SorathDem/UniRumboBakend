using Microsoft.EntityFrameworkCore;

namespace UniRumbo.Repositories;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alojamiento> Alojamiento { get; set; }
    public DbSet<Rutum> Rutas { get; set; }
    public virtual DbSet<Estado> Estados { get; set; }
    public virtual DbSet<Rol> Rol { get; set; }
    public virtual DbSet<Sede> Sede { get; set; }
    public virtual DbSet<Usuario> Usuario { get; set; }
    public virtual DbSet<Vehiculo> Vehiculos { get; set; }
    public DbSet<SolicitudRutum> SolicitudRuta { get; set; }
    public DbSet<SolicitudAlojamiento> SolicitudAlojamiento { get; set; }
    public DbSet<Estado> Estado { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rutum>(entity =>
        {
            entity.HasKey(e => e.IdRuta);

            entity.ToTable("Ruta");
            // ... Tu código de mapeo de columnas existente ...
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.IdVehiculo).HasColumnName("id_vehiculo");

            // 1. Relación con Usuario
            entity.HasOne(r => r.Usuario)
                  .WithMany(u => u.Ruta)
                  .HasForeignKey(r => r.IdUsuario)
                  .IsRequired();

            entity.HasOne(r => r.Vehiculo) // Propiedad de Navegación en Rutum
                                           // DEBES APUNTAR A LA PROPIEDAD 'Rutas' EN LA ENTIDAD VEHICULO
                      .WithMany(v => v.Rutas)
                      .HasForeignKey(r => r.IdVehiculo) // Usa la propiedad IdVehiculo como FK
                      .IsRequired();
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
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Ubicacion)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ubicacion");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Alojamientos)
                .HasForeignKey(d => d.IdUsuario)
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

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PK__Estado__...");
            entity.ToTable("Estado");

            entity.Property(e => e.IdEstado).HasColumnName("id_estado");

            // 👇 importante: mapea tu propiedad Estado1 a la columna "estado"
            entity.Property(e => e.EstadoNombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("estado");

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
