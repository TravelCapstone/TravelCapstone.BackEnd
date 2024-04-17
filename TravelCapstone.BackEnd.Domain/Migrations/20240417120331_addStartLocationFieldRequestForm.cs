using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class addStartLocationFieldRequestForm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrivateTourRequests_VehicleTypes_MainVehicleId",
                table: "PrivateTourRequests");

            migrationBuilder.DropIndex(
                name: "IX_PrivateTourRequests_MainVehicleId",
                table: "PrivateTourRequests");

            migrationBuilder.DropColumn(
                name: "MainVehicleId",
                table: "PrivateTourRequests");

            migrationBuilder.AddColumn<string>(
                name: "StartLocation",
                table: "PrivateTourRequests",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartLocation",
                table: "PrivateTourRequests");

            migrationBuilder.AddColumn<int>(
                name: "MainVehicleId",
                table: "PrivateTourRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PrivateTourRequests_MainVehicleId",
                table: "PrivateTourRequests",
                column: "MainVehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateTourRequests_VehicleTypes_MainVehicleId",
                table: "PrivateTourRequests",
                column: "MainVehicleId",
                principalTable: "VehicleTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
