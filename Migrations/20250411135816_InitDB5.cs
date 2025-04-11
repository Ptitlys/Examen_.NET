using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Examen.Migrations
{
    /// <inheritdoc />
    public partial class InitDB5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
