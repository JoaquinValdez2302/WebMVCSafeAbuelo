using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebMVCSafeAbuelo.Migrations
{
    /// <inheritdoc />
    public partial class AgregandoMetodologias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MetodologiaAtaque",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    PrincipalMotorPsicologico = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SeñalesDeAlarma = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AccionPreventiva = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    EstaActivo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetodologiaAtaque", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MetodologiaAtaque");
        }
    }
}
