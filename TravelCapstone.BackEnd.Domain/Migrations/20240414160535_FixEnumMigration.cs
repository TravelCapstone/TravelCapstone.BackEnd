using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class FixEnumMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AttendanceRouteTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "IN" },
                    { 1, "OUT" }
                });

            migrationBuilder.InsertData(
                table: "AttendanceTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "NOTYET" },
                    { 1, "ATTENDEDED" },
                    { 2, "ABSENT" }
                });

            migrationBuilder.InsertData(
                table: "JoinTourStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "NEW" },
                    { 1, "APPROVED" },
                    { 2, "REJECTED" }
                });

            migrationBuilder.InsertData(
                table: "MaterialTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 0, "PASSPORT" });

            migrationBuilder.InsertData(
                table: "OptionClasses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "ECONOMY" },
                    { 1, "MIDDLE" },
                    { 2, "PREMIUM" }
                });

            migrationBuilder.InsertData(
                table: "OptionQuotationStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "NEW" },
                    { 1, "ACTIVE" },
                    { 2, "IN_ACTIVE" }
                });

            migrationBuilder.InsertData(
                table: "OrderStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "NEW" },
                    { 1, "PAID" }
                });

            migrationBuilder.InsertData(
                table: "PrivateTourStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "NEW" },
                    { 1, "WAITINGFORCUSTOMER" },
                    { 2, "APPROVED" },
                    { 3, "REJECTED" }
                });

            migrationBuilder.InsertData(
                table: "ServiceTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "HOTEL" },
                    { 1, "RESTAURANTS" },
                    { 2, "ENTERTAINMENT" },
                    { 3, "VEHICLE_SUPPLY" },
                    { 4, "AIR_TICKET_SUPPLY" }
                });

            migrationBuilder.InsertData(
                table: "TourStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "NEW" },
                    { 1, "PENDING" },
                    { 2, "SUFFICIENT" },
                    { 3, "SCHEDULING_COMPLETED" },
                    { 4, "STARTING" },
                    { 5, "COMPLETED" }
                });

            migrationBuilder.InsertData(
                table: "TourTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "PUBLIC" },
                    { 1, "CUSTOM" },
                    { 2, "ENTERPRISE" }
                });

            migrationBuilder.InsertData(
                table: "TransactionTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "RECHARGE_FROM_VNPAY" },
                    { 1, "RECHARGE_FROM_MOMO" },
                    { 2, "PAYMENT_FOR_SERVICES" }
                });

            migrationBuilder.InsertData(
                table: "Units",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "DAY" },
                    { 1, "ROOM" },
                    { 2, "PERSON" }
                });

            migrationBuilder.InsertData(
                table: "VehicleTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 0, "BUS" });

            migrationBuilder.InsertData(
                table: "VehicleTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "CAR" },
                    { 2, "PLANE" },
                    { 3, "BOAT" },
                    { 4, "BICYCLE" },
                    { 5, "HELICOPTER" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AttendanceRouteTypes",
                keyColumn: "Id",
                keyValue: 0);

            migrationBuilder.DeleteData(
                table: "AttendanceRouteTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AttendanceTypes",
                keyColumn: "Id",
                keyValue: 0);

            migrationBuilder.DeleteData(
                table: "AttendanceTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AttendanceTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "JoinTourStatuses",
                keyColumn: "Id",
                keyValue: 0);

            migrationBuilder.DeleteData(
                table: "JoinTourStatuses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "JoinTourStatuses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MaterialTypes",
                keyColumn: "Id",
                keyValue: 0);

            migrationBuilder.DeleteData(
                table: "OptionClasses",
                keyColumn: "Id",
                keyValue: 0);

            migrationBuilder.DeleteData(
                table: "OptionClasses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OptionClasses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OptionQuotationStatuses",
                keyColumn: "Id",
                keyValue: 0);

            migrationBuilder.DeleteData(
                table: "OptionQuotationStatuses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OptionQuotationStatuses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OrderStatuses",
                keyColumn: "Id",
                keyValue: 0);

            migrationBuilder.DeleteData(
                table: "OrderStatuses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PrivateTourStatuses",
                keyColumn: "Id",
                keyValue: 0);

            migrationBuilder.DeleteData(
                table: "PrivateTourStatuses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PrivateTourStatuses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PrivateTourStatuses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: 0);

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TourStatuses",
                keyColumn: "Id",
                keyValue: 0);

            migrationBuilder.DeleteData(
                table: "TourStatuses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TourStatuses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TourStatuses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TourStatuses",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TourStatuses",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TourTypes",
                keyColumn: "Id",
                keyValue: 0);

            migrationBuilder.DeleteData(
                table: "TourTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TourTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TransactionTypes",
                keyColumn: "Id",
                keyValue: 0);

            migrationBuilder.DeleteData(
                table: "TransactionTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TransactionTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 0);

            migrationBuilder.DeleteData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "VehicleTypes",
                keyColumn: "Id",
                keyValue: 0);

            migrationBuilder.DeleteData(
                table: "VehicleTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "VehicleTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "VehicleTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "VehicleTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "VehicleTypes",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
