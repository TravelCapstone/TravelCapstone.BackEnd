using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class UpdateUnit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCostHistorys_Units_UnitId",
                table: "ServiceCostHistorys");

            migrationBuilder.DropIndex(
                name: "IX_ServiceCostHistorys_UnitId",
                table: "ServiceCostHistorys");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "ServiceCostHistorys");

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "Services",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Services_UnitId",
                table: "Services",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Units_UnitId",
                table: "Services",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Units_UnitId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_UnitId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Services");

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "ServiceCostHistorys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCostHistorys_UnitId",
                table: "ServiceCostHistorys",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCostHistorys_Units_UnitId",
                table: "ServiceCostHistorys",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
