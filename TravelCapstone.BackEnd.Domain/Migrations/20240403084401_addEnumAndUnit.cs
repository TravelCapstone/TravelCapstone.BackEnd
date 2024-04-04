using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class addEnumAndUnit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TourGuideId",
                table: "Tours",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Unit",
                table: "ServiceCostHistorys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tours_TourGuideId",
                table: "Tours",
                column: "TourGuideId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tours_AspNetUsers_TourGuideId",
                table: "Tours",
                column: "TourGuideId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tours_AspNetUsers_TourGuideId",
                table: "Tours");

            migrationBuilder.DropIndex(
                name: "IX_Tours_TourGuideId",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "TourGuideId",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "ServiceCostHistorys");
        }
    }
}