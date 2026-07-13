using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebMVCSafeAbuelo.Migrations
{
    /// <inheritdoc />
    public partial class IntegrarPerfilesUsuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PerfilesUsuarios",
                columns: table => new
                {
                    FirebaseUid = table.Column<string>(type: "text", nullable: false),
                    NombreCompleto = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    EmailContacto = table.Column<string>(type: "text", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfilesUsuarios", x => x.FirebaseUid);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportesIncidentes_AuthorId",
                table: "ReportesIncidentes",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportesIncidentes_PerfilesUsuarios_AuthorId",
                table: "ReportesIncidentes",
                column: "AuthorId",
                principalTable: "PerfilesUsuarios",
                principalColumn: "FirebaseUid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportesIncidentes_PerfilesUsuarios_AuthorId",
                table: "ReportesIncidentes");

            migrationBuilder.DropTable(
                name: "PerfilesUsuarios");

            migrationBuilder.DropIndex(
                name: "IX_ReportesIncidentes_AuthorId",
                table: "ReportesIncidentes");
        }
    }
}
