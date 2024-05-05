using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class SetEndPointOfVehicleQuotationToNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestedLocations_Districts_DistrictId",
                table: "RequestedLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleQuotationDetails_Provinces_EndPointId",
                table: "VehicleQuotationDetails");

            migrationBuilder.DropIndex(
                name: "IX_RequestedLocations_DistrictId",
                table: "RequestedLocations");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "RequestedLocations");

            migrationBuilder.AlterColumn<Guid>(
                name: "EndPointId",
                table: "VehicleQuotationDetails",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "EndPointDistrictId",
                table: "VehicleQuotationDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StartPointDistrictId",
                table: "VehicleQuotationDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleQuotationDetails_EndPointDistrictId",
                table: "VehicleQuotationDetails",
                column: "EndPointDistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleQuotationDetails_StartPointDistrictId",
                table: "VehicleQuotationDetails",
                column: "StartPointDistrictId");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleQuotationDetails_Districts_EndPointDistrictId",
                table: "VehicleQuotationDetails",
                column: "EndPointDistrictId",
                principalTable: "Districts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleQuotationDetails_Districts_StartPointDistrictId",
                table: "VehicleQuotationDetails",
                column: "StartPointDistrictId",
                principalTable: "Districts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleQuotationDetails_Provinces_EndPointId",
                table: "VehicleQuotationDetails",
                column: "EndPointId",
                principalTable: "Provinces",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleQuotationDetails_Districts_EndPointDistrictId",
                table: "VehicleQuotationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleQuotationDetails_Districts_StartPointDistrictId",
                table: "VehicleQuotationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleQuotationDetails_Provinces_EndPointId",
                table: "VehicleQuotationDetails");

            migrationBuilder.DropIndex(
                name: "IX_VehicleQuotationDetails_EndPointDistrictId",
                table: "VehicleQuotationDetails");

            migrationBuilder.DropIndex(
                name: "IX_VehicleQuotationDetails_StartPointDistrictId",
                table: "VehicleQuotationDetails");

            migrationBuilder.DropColumn(
                name: "EndPointDistrictId",
                table: "VehicleQuotationDetails");

            migrationBuilder.DropColumn(
                name: "StartPointDistrictId",
                table: "VehicleQuotationDetails");

            migrationBuilder.AlterColumn<Guid>(
                name: "EndPointId",
                table: "VehicleQuotationDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DistrictId",
                table: "RequestedLocations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_RequestedLocations_DistrictId",
                table: "RequestedLocations",
                column: "DistrictId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestedLocations_Districts_DistrictId",
                table: "RequestedLocations",
                column: "DistrictId",
                principalTable: "Districts",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleQuotationDetails_Provinces_EndPointId",
                table: "VehicleQuotationDetails",
                column: "EndPointId",
                principalTable: "Provinces",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
