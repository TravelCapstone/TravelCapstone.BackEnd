using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class AddFieldToVehicleRouteDriverReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ReferenceTransportPriceId",
                table: "VehicleRoutes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ArrivalDate",
                table: "ReferenceTransportPrices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleRoutes_ReferenceTransportPriceId",
                table: "VehicleRoutes",
                column: "ReferenceTransportPriceId");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleRoutes_ReferenceTransportPrices_ReferenceTransportPriceId",
                table: "VehicleRoutes",
                column: "ReferenceTransportPriceId",
                principalTable: "ReferenceTransportPrices",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleRoutes_ReferenceTransportPrices_ReferenceTransportPriceId",
                table: "VehicleRoutes");

            migrationBuilder.DropIndex(
                name: "IX_VehicleRoutes_ReferenceTransportPriceId",
                table: "VehicleRoutes");

            migrationBuilder.DropColumn(
                name: "ReferenceTransportPriceId",
                table: "VehicleRoutes");

            migrationBuilder.DropColumn(
                name: "ArrivalDate",
                table: "ReferenceTransportPrices");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Drivers");
        }
    }
}
