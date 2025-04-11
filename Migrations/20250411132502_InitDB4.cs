using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Examen.Migrations
{
    /// <inheritdoc />
    public partial class InitDB4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InterventionCustomer");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Interventions",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "Interventions",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduledFor",
                table: "Interventions",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Interventions_CustomerId",
                table: "Interventions",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Interventions_AspNetUsers_CustomerId",
                table: "Interventions",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interventions_AspNetUsers_CustomerId",
                table: "Interventions");

            migrationBuilder.DropIndex(
                name: "IX_Interventions_CustomerId",
                table: "Interventions");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Interventions");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Interventions");

            migrationBuilder.DropColumn(
                name: "ScheduledFor",
                table: "Interventions");

            migrationBuilder.CreateTable(
                name: "InterventionCustomer",
                columns: table => new
                {
                    InterventionId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterventionCustomer", x => new { x.InterventionId, x.CustomerId });
                    table.ForeignKey(
                        name: "FK_InterventionCustomer_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterventionCustomer_Interventions_InterventionId",
                        column: x => x.InterventionId,
                        principalTable: "Interventions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_InterventionCustomer_CustomerId",
                table: "InterventionCustomer",
                column: "CustomerId");
        }
    }
}
