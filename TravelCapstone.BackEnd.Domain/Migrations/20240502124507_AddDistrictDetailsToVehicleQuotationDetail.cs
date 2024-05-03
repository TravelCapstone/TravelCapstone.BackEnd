using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class AddDistrictDetailsToVehicleQuotationDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleQuotationDetails_Districts_EndPointDistrictId",
                table: "VehicleQuotationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleQuotationDetails_Districts_StartPointDistrictId",
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
        }
    }
}
