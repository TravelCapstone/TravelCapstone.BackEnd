using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class ReplaceStartEndDateWithDaysinVehicleQuotation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "VehicleQuotationDetails");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "VehicleQuotationDetails");

            migrationBuilder.AddColumn<int>(
                name: "NumOfRentingDay",
                table: "VehicleQuotationDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumOfRentingDay",
                table: "VehicleQuotationDetails");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "VehicleQuotationDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "VehicleQuotationDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
