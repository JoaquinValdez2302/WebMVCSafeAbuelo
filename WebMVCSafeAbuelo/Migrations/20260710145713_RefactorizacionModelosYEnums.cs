using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebMVCSafeAbuelo.Migrations
{
    /// <inheritdoc />
    public partial class RefactorizacionModelosYEnums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "ReportesIncidentes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LinkEvidencia",
                table: "EvidenciasIncidentes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notas",
                table: "EvidenciasIncidentes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "ReportesIncidentes");

            migrationBuilder.DropColumn(
                name: "LinkEvidencia",
                table: "EvidenciasIncidentes");

            migrationBuilder.DropColumn(
                name: "Notas",
                table: "EvidenciasIncidentes");
        }
    }
}
