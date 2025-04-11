using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Examen.Migrations
{
    /// <inheritdoc />
    public partial class InitDB8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Interventions_InterventionId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_InterventionId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "InterventionId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "InterventionTechnician",
                columns: table => new
                {
                    InterventionsId = table.Column<int>(type: "int", nullable: false),
                    TechniciansId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterventionTechnician", x => new { x.InterventionsId, x.TechniciansId });
                    table.ForeignKey(
                        name: "FK_InterventionTechnician_AspNetUsers_TechniciansId",
                        column: x => x.TechniciansId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterventionTechnician_Interventions_InterventionsId",
                        column: x => x.InterventionsId,
                        principalTable: "Interventions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_InterventionTechnician_TechniciansId",
                table: "InterventionTechnician",
                column: "TechniciansId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InterventionTechnician");

            migrationBuilder.AddColumn<int>(
                name: "InterventionId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_InterventionId",
                table: "AspNetUsers",
                column: "InterventionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Interventions_InterventionId",
                table: "AspNetUsers",
                column: "InterventionId",
                principalTable: "Interventions",
                principalColumn: "Id");
        }
    }
}
