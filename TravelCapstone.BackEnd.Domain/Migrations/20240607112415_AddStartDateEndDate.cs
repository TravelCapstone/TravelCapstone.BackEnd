using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class AddStartDateEndDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Routes");

            migrationBuilder.AddColumn<Guid>(
                name: "EndPortId",
                table: "VehicleQuotationDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StartPortId",
                table: "VehicleQuotationDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "TourguideAssignments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "TourguideAssignments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_VehicleQuotationDetails_EndPortId",
                table: "VehicleQuotationDetails",
                column: "EndPortId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleQuotationDetails_StartPortId",
                table: "VehicleQuotationDetails",
                column: "StartPortId");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleQuotationDetails_Ports_EndPortId",
                table: "VehicleQuotationDetails",
                column: "EndPortId",
                principalTable: "Ports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleQuotationDetails_Ports_StartPortId",
                table: "VehicleQuotationDetails",
                column: "StartPortId",
                principalTable: "Ports",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleQuotationDetails_Ports_EndPortId",
                table: "VehicleQuotationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleQuotationDetails_Ports_StartPortId",
                table: "VehicleQuotationDetails");

            migrationBuilder.DropIndex(
                name: "IX_VehicleQuotationDetails_EndPortId",
                table: "VehicleQuotationDetails");

            migrationBuilder.DropIndex(
                name: "IX_VehicleQuotationDetails_StartPortId",
                table: "VehicleQuotationDetails");

            migrationBuilder.DropColumn(
                name: "EndPortId",
                table: "VehicleQuotationDetails");

            migrationBuilder.DropColumn(
                name: "StartPortId",
                table: "VehicleQuotationDetails");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "TourguideAssignments");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "TourguideAssignments");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Routes",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
