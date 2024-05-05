using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class AddSalary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumofVehilce",
                table: "VehicleQuotationDetails",
                newName: "NumOfVehicle");

            migrationBuilder.AddColumn<double>(
                name: "TourGuideSalary",
                table: "TourguideAssignments",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "FixDriverSalary",
                table: "Drivers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Drivers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TourGuideSalary",
                table: "TourguideAssignments");

            migrationBuilder.DropColumn(
                name: "FixDriverSalary",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Drivers");

            migrationBuilder.RenameColumn(
                name: "NumOfVehicle",
                table: "VehicleQuotationDetails",
                newName: "NumofVehilce");
        }
    }
}
