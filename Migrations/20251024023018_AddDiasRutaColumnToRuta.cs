using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniRumboBakend.Migrations
{
    /// <inheritdoc />
    public partial class AddDiasRutaColumnToRuta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Estado",
                columns: table => new
                {
                    id_estado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    estado = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Estado__...", x => x.id_estado);
                });

            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    id_rol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Rol__3214EC0723456789", x => x.id_rol);
                });

            migrationBuilder.CreateTable(
                name: "Sede",
                columns: table => new
                {
                    id_sede = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Sede__3214EC07ABCDEF12", x => x.id_sede);
                });

            migrationBuilder.CreateTable(
                name: "Vehiculo",
                columns: table => new
                {
                    id_vehiculo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tipo_vehiculo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehiculo", x => x.id_vehiculo);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    apellido = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    numero = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    correo = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    contrasena = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    id_rol = table.Column<int>(type: "int", nullable: false),
                    id_sede = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Usuario__4E3E04AD12345678", x => x.id_usuario);
                    table.ForeignKey(
                        name: "FK__Usuario__id_rol__2A4B4B5E",
                        column: x => x.id_rol,
                        principalTable: "Rol",
                        principalColumn: "id_rol");
                    table.ForeignKey(
                        name: "FK__Usuario__id_sede__29572725",
                        column: x => x.id_sede,
                        principalTable: "Sede",
                        principalColumn: "id_sede");
                });

            migrationBuilder.CreateTable(
                name: "Alojamiento",
                columns: table => new
                {
                    id_alojamiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ubicacion = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    descripcion = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    id_usuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Alojamie__2908794AF1C66E30", x => x.id_alojamiento);
                    table.ForeignKey(
                        name: "FK__Alojamien__id_us__34C8D9D1",
                        column: x => x.id_usuario,
                        principalTable: "Usuario",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "Ruta",
                columns: table => new
                {
                    id_ruta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    punto_origen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    punto_destino = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    hora_salida = table.Column<DateTime>(type: "datetime2", nullable: false),
                    hora_regreso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    cupos_ida = table.Column<int>(type: "int", nullable: false),
                    cupos_vuelta = table.Column<int>(type: "int", nullable: false),
                    id_usuario = table.Column<int>(type: "int", nullable: false),
                    id_vehiculo = table.Column<int>(type: "int", nullable: false),
                    dias_ruta = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ruta", x => x.id_ruta);
                    table.ForeignKey(
                        name: "FK_Ruta_Usuario_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "Usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ruta_Vehiculo_id_vehiculo",
                        column: x => x.id_vehiculo,
                        principalTable: "Vehiculo",
                        principalColumn: "id_vehiculo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SolicitudAlojamientos",
                columns: table => new
                {
                    IdSoliAlojamiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAlojamiento = table.Column<int>(type: "int", nullable: false),
                    IdEstado = table.Column<int>(type: "int", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdAlojamientoNavigationIdAlojamiento = table.Column<int>(type: "int", nullable: false),
                    IdEstadoNavigationIdEstado = table.Column<int>(type: "int", nullable: false),
                    IdUsuarioNavigationIdUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudAlojamientos", x => x.IdSoliAlojamiento);
                    table.ForeignKey(
                        name: "FK_SolicitudAlojamientos_Alojamiento_IdAlojamientoNavigationIdAlojamiento",
                        column: x => x.IdAlojamientoNavigationIdAlojamiento,
                        principalTable: "Alojamiento",
                        principalColumn: "id_alojamiento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SolicitudAlojamientos_Estado_IdEstadoNavigationIdEstado",
                        column: x => x.IdEstadoNavigationIdEstado,
                        principalTable: "Estado",
                        principalColumn: "id_estado",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SolicitudAlojamientos_Usuario_IdUsuarioNavigationIdUsuario",
                        column: x => x.IdUsuarioNavigationIdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Solicitud_ruta",
                columns: table => new
                {
                    id_SoliRuta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_estado = table.Column<int>(type: "int", nullable: false),
                    id_usuario = table.Column<int>(type: "int", nullable: false),
                    id_ruta = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitud_ruta", x => x.id_SoliRuta);
                    table.ForeignKey(
                        name: "FK_Solicitud_ruta_Estado_id_estado",
                        column: x => x.id_estado,
                        principalTable: "Estado",
                        principalColumn: "id_estado",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Solicitud_ruta_Ruta_id_ruta",
                        column: x => x.id_ruta,
                        principalTable: "Ruta",
                        principalColumn: "id_ruta",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Solicitud_ruta_Usuario_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "Usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alojamiento_id_usuario",
                table: "Alojamiento",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Ruta_id_usuario",
                table: "Ruta",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Ruta_id_vehiculo",
                table: "Ruta",
                column: "id_vehiculo");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitud_ruta_id_estado",
                table: "Solicitud_ruta",
                column: "id_estado");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitud_ruta_id_ruta",
                table: "Solicitud_ruta",
                column: "id_ruta");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitud_ruta_id_usuario",
                table: "Solicitud_ruta",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudAlojamientos_IdAlojamientoNavigationIdAlojamiento",
                table: "SolicitudAlojamientos",
                column: "IdAlojamientoNavigationIdAlojamiento");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudAlojamientos_IdEstadoNavigationIdEstado",
                table: "SolicitudAlojamientos",
                column: "IdEstadoNavigationIdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudAlojamientos_IdUsuarioNavigationIdUsuario",
                table: "SolicitudAlojamientos",
                column: "IdUsuarioNavigationIdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_id_rol",
                table: "Usuario",
                column: "id_rol");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_id_sede",
                table: "Usuario",
                column: "id_sede");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Solicitud_ruta");

            migrationBuilder.DropTable(
                name: "SolicitudAlojamientos");

            migrationBuilder.DropTable(
                name: "Ruta");

            migrationBuilder.DropTable(
                name: "Alojamiento");

            migrationBuilder.DropTable(
                name: "Estado");

            migrationBuilder.DropTable(
                name: "Vehiculo");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Rol");

            migrationBuilder.DropTable(
                name: "Sede");
        }
    }
}
