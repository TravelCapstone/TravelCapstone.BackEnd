using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class updateEnumServiceAvailability : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePerAdult",
                table: "ServiceCostHistorys");

            migrationBuilder.DropColumn(
                name: "PricePerAdult",
                table: "SellPriceHistorys");

            migrationBuilder.RenameColumn(
                name: "PricePerChild",
                table: "ServiceCostHistorys",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "PricePerChild",
                table: "SellPriceHistorys",
                newName: "Price");

            migrationBuilder.AddColumn<int>(
                name: "ServiceAvailabilityId",
                table: "Services",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ServingQuantity",
                table: "Services",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "SurchargePercent",
                table: "Services",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "ServiceAvailabilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceAvailabilities", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ServiceAvailabilities",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "ADULT" },
                    { 1, "CHILD" },
                    { 2, "BOTH" }
                });

            migrationBuilder.UpdateData(
                table: "VehicleTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "COACH");

            migrationBuilder.UpdateData(
                table: "VehicleTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "LIMOUSINE");

            migrationBuilder.UpdateData(
                table: "VehicleTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "CAR");

            migrationBuilder.UpdateData(
                table: "VehicleTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "PLANE");

            migrationBuilder.UpdateData(
                table: "VehicleTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "BOAT");

            migrationBuilder.InsertData(
                table: "VehicleTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 6, "BICYCLE" },
                    { 7, "HELICOPTER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServiceAvailabilityId",
                table: "Services",
                column: "ServiceAvailabilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ServiceAvailabilities_ServiceAvailabilityId",
                table: "Services",
                column: "ServiceAvailabilityId",
                principalTable: "ServiceAvailabilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_ServiceAvailabilities_ServiceAvailabilityId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "ServiceAvailabilities");

            migrationBuilder.DropIndex(
                name: "IX_Services_ServiceAvailabilityId",
                table: "Services");

            migrationBuilder.DeleteData(
                table: "VehicleTypes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "VehicleTypes",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DropColumn(
                name: "ServiceAvailabilityId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ServingQuantity",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "SurchargePercent",
                table: "Services");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "ServiceCostHistorys",
                newName: "PricePerChild");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "SellPriceHistorys",
                newName: "PricePerChild");

            migrationBuilder.AddColumn<double>(
                name: "PricePerAdult",
                table: "ServiceCostHistorys",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PricePerAdult",
                table: "SellPriceHistorys",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "VehicleTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "CAR");

            migrationBuilder.UpdateData(
                table: "VehicleTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "PLANE");

            migrationBuilder.UpdateData(
                table: "VehicleTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "BOAT");

            migrationBuilder.UpdateData(
                table: "VehicleTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "BICYCLE");

            migrationBuilder.UpdateData(
                table: "VehicleTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "HELICOPTER");
        }
    }
}
