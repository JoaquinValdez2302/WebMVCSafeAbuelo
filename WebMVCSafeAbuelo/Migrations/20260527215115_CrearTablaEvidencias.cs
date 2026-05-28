using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebMVCSafeAbuelo.Migrations
{
    /// <inheritdoc />
    public partial class CrearTablaEvidencias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EvidenciasIncidentes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tipo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Valor = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Notas = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    ReporteIncidenteId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvidenciasIncidentes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvidenciasIncidentes_ReportesIncidentes_ReporteIncidenteId",
                        column: x => x.ReporteIncidenteId,
                        principalTable: "ReportesIncidentes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EvidenciasIncidentes_ReporteIncidenteId",
                table: "EvidenciasIncidentes",
                column: "ReporteIncidenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EvidenciasIncidentes");
        }
    }
}
