using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VIMOD.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AddModulosYProgresoEstudiante : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ModulosCurso",
                columns: table => new
                {
                    IdModulo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    IdCurso = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModulosCurso", x => x.IdModulo);
                    table.ForeignKey(
                        name: "FK_ModulosCurso_Cursos_IdCurso",
                        column: x => x.IdCurso,
                        principalTable: "Cursos",
                        principalColumn: "IdCurso",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContenidosModulo",
                columns: table => new
                {
                    IdContenido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    URLContenido = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    IdModulo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContenidosModulo", x => x.IdContenido);
                    table.ForeignKey(
                        name: "FK_ContenidosModulo_ModulosCurso_IdModulo",
                        column: x => x.IdModulo,
                        principalTable: "ModulosCurso",
                        principalColumn: "IdModulo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProgresosEstudiante",
                columns: table => new
                {
                    IdProgreso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdContenido = table.Column<int>(type: "int", nullable: false),
                    Completado = table.Column<bool>(type: "bit", nullable: false),
                    FechaCompletado = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgresosEstudiante", x => x.IdProgreso);
                    table.ForeignKey(
                        name: "FK_ProgresosEstudiante_AspNetUsers_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProgresosEstudiante_ContenidosModulo_IdContenido",
                        column: x => x.IdContenido,
                        principalTable: "ContenidosModulo",
                        principalColumn: "IdContenido",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContenidosModulo_IdModulo",
                table: "ContenidosModulo",
                column: "IdModulo");

            migrationBuilder.CreateIndex(
                name: "IX_ModulosCurso_IdCurso",
                table: "ModulosCurso",
                column: "IdCurso");

            migrationBuilder.CreateIndex(
                name: "IX_ProgresosEstudiante_IdContenido",
                table: "ProgresosEstudiante",
                column: "IdContenido");

            migrationBuilder.CreateIndex(
                name: "IX_ProgresosEstudiante_IdUsuario",
                table: "ProgresosEstudiante",
                column: "IdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProgresosEstudiante");

            migrationBuilder.DropTable(
                name: "ContenidosModulo");

            migrationBuilder.DropTable(
                name: "ModulosCurso");
        }
    }
}
