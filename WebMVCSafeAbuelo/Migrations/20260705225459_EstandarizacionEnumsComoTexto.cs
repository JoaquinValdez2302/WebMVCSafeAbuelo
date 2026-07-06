using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebMVCSafeAbuelo.Migrations
{
    /// <inheritdoc />
    public partial class EstandarizacionEnumsComoTexto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PlataformaDeContacto",
                table: "ReportesIncidentes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "PlataformaOtra",
                table: "ReportesIncidentes",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Tipo",
                table: "EvidenciasIncidentes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlataformaOtra",
                table: "ReportesIncidentes");

            migrationBuilder.AlterColumn<string>(
                name: "PlataformaDeContacto",
                table: "ReportesIncidentes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Tipo",
                table: "EvidenciasIncidentes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
