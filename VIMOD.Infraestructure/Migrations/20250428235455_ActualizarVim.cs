using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VIMOD.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarVim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoUnico",
                table: "Certificados",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoUnico",
                table: "Certificados");
        }
    }
}
