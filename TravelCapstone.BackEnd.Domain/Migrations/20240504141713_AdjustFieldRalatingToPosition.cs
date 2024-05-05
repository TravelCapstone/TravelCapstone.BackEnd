using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class AdjustFieldRalatingToPosition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Tours_TourId",
                table: "Materials");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Facilities_EndPointId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleRoutes_Drivers_DriverId",
                table: "VehicleRoutes");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleRoutes_ReferenceTransportPrices_ReferenceTransportPriceId",
                table: "VehicleRoutes");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleRoutes_Vehicles_VehicleId",
                table: "VehicleRoutes");

            migrationBuilder.DropIndex(
                name: "IX_VehicleRoutes_ReferenceTransportPriceId",
                table: "VehicleRoutes");

            migrationBuilder.DropIndex(
                name: "IX_Materials_TourId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "ReferenceTransportPriceId",
                table: "VehicleRoutes");

            migrationBuilder.DropColumn(
                name: "QuantityOfAdult",
                table: "PlanServiceCostDetails");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "TourId",
                table: "Materials");

            migrationBuilder.RenameColumn(
                name: "QuantityOfChild",
                table: "PlanServiceCostDetails",
                newName: "Quantity");

            migrationBuilder.AlterColumn<Guid>(
                name: "VehicleId",
                table: "VehicleRoutes",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "DriverId",
                table: "VehicleRoutes",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "ReferenceBrandName",
                table: "VehicleRoutes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VehicleType",
                table: "VehicleRoutes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "TourId",
                table: "TourguideAssignments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "EndPointId",
                table: "Routes",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "PortEndPointId",
                table: "Routes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PortStartPointId",
                table: "Routes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DistrictId",
                table: "RequestedLocations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "PlanServiceCostDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "ReferenceTransportPriceId",
                table: "PlanServiceCostDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "PlanServiceCostDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Materials",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Materials",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "DayPlans",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MaterialAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TourId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialAssignments_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_MaterialAssignments_Tours_TourId",
                        column: x => x.TourId,
                        principalTable: "Tours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TourguideAssignments_TourId",
                table: "TourguideAssignments",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_PortEndPointId",
                table: "Routes",
                column: "PortEndPointId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_PortStartPointId",
                table: "Routes",
                column: "PortStartPointId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestedLocations_DistrictId",
                table: "RequestedLocations",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanServiceCostDetails_ReferenceTransportPriceId",
                table: "PlanServiceCostDetails",
                column: "ReferenceTransportPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialAssignments_MaterialId",
                table: "MaterialAssignments",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialAssignments_TourId",
                table: "MaterialAssignments",
                column: "TourId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanServiceCostDetails_ReferenceTransportPrices_ReferenceTransportPriceId",
                table: "PlanServiceCostDetails",
                column: "ReferenceTransportPriceId",
                principalTable: "ReferenceTransportPrices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestedLocations_Districts_DistrictId",
                table: "RequestedLocations",
                column: "DistrictId",
                principalTable: "Districts",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Facilities_EndPointId",
                table: "Routes",
                column: "EndPointId",
                principalTable: "Facilities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Ports_PortEndPointId",
                table: "Routes",
                column: "PortEndPointId",
                principalTable: "Ports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Ports_PortStartPointId",
                table: "Routes",
                column: "PortStartPointId",
                principalTable: "Ports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TourguideAssignments_Tours_TourId",
                table: "TourguideAssignments",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleRoutes_Drivers_DriverId",
                table: "VehicleRoutes",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleRoutes_Vehicles_VehicleId",
                table: "VehicleRoutes",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanServiceCostDetails_ReferenceTransportPrices_ReferenceTransportPriceId",
                table: "PlanServiceCostDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestedLocations_Districts_DistrictId",
                table: "RequestedLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Facilities_EndPointId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Ports_PortEndPointId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Ports_PortStartPointId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_TourguideAssignments_Tours_TourId",
                table: "TourguideAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleRoutes_Drivers_DriverId",
                table: "VehicleRoutes");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleRoutes_Vehicles_VehicleId",
                table: "VehicleRoutes");

            migrationBuilder.DropTable(
                name: "MaterialAssignments");

            migrationBuilder.DropIndex(
                name: "IX_TourguideAssignments_TourId",
                table: "TourguideAssignments");

            migrationBuilder.DropIndex(
                name: "IX_Routes_PortEndPointId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_PortStartPointId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_RequestedLocations_DistrictId",
                table: "RequestedLocations");

            migrationBuilder.DropIndex(
                name: "IX_PlanServiceCostDetails_ReferenceTransportPriceId",
                table: "PlanServiceCostDetails");

            migrationBuilder.DropColumn(
                name: "ReferenceBrandName",
                table: "VehicleRoutes");

            migrationBuilder.DropColumn(
                name: "VehicleType",
                table: "VehicleRoutes");

            migrationBuilder.DropColumn(
                name: "TourId",
                table: "TourguideAssignments");

            migrationBuilder.DropColumn(
                name: "PortEndPointId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "PortStartPointId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "RequestedLocations");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "PlanServiceCostDetails");

            migrationBuilder.DropColumn(
                name: "ReferenceTransportPriceId",
                table: "PlanServiceCostDetails");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "PlanServiceCostDetails");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "DayPlans");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "PlanServiceCostDetails",
                newName: "QuantityOfChild");

            migrationBuilder.AlterColumn<Guid>(
                name: "VehicleId",
                table: "VehicleRoutes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "DriverId",
                table: "VehicleRoutes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ReferenceTransportPriceId",
                table: "VehicleRoutes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "EndPointId",
                table: "Routes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuantityOfAdult",
                table: "PlanServiceCostDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Materials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "TourId",
                table: "Materials",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_VehicleRoutes_ReferenceTransportPriceId",
                table: "VehicleRoutes",
                column: "ReferenceTransportPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_TourId",
                table: "Materials",
                column: "TourId");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Tours_TourId",
                table: "Materials",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Facilities_EndPointId",
                table: "Routes",
                column: "EndPointId",
                principalTable: "Facilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleRoutes_Drivers_DriverId",
                table: "VehicleRoutes",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleRoutes_ReferenceTransportPrices_ReferenceTransportPriceId",
                table: "VehicleRoutes",
                column: "ReferenceTransportPriceId",
                principalTable: "ReferenceTransportPrices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleRoutes_Vehicles_VehicleId",
                table: "VehicleRoutes",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
