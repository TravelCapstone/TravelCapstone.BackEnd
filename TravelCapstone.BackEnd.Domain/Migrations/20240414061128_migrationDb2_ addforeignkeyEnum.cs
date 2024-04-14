using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class migrationDb2_addforeignkeyEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VehicleType",
                table: "Vehicles",
                newName: "VehicleTypeId");

            migrationBuilder.RenameColumn(
                name: "TransactionType",
                table: "Transactions",
                newName: "TransactionTypeId");

            migrationBuilder.RenameColumn(
                name: "TourType",
                table: "Tours",
                newName: "VehicleTypeId");

            migrationBuilder.RenameColumn(
                name: "TourStatus",
                table: "Tours",
                newName: "TourTypeId");

            migrationBuilder.RenameColumn(
                name: "MainVehicle",
                table: "Tours",
                newName: "TourStatusId");

            migrationBuilder.RenameColumn(
                name: "Unit",
                table: "ServiceCostHistorys",
                newName: "UnitId");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "PrivateTourRequests",
                newName: "PrivateTourStatusId");

            migrationBuilder.RenameColumn(
                name: "MainVehicle",
                table: "PrivateTourRequests",
                newName: "MainVehicleId");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "PrivateJoinTourRequests",
                newName: "JoinTourStatusId");

            migrationBuilder.RenameColumn(
                name: "OrderStatus",
                table: "Orders",
                newName: "OrderStatusId");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "OptionQuotations",
                newName: "OptionQuotationStatusId");

            migrationBuilder.RenameColumn(
                name: "OptionClass",
                table: "OptionQuotations",
                newName: "OptionClassId");

            migrationBuilder.RenameColumn(
                name: "MaterialType",
                table: "Materials",
                newName: "MaterialTypeId");

            migrationBuilder.RenameColumn(
                name: "AttendanceRouteType",
                table: "AttendanceRoutes",
                newName: "AttendanceRouteTypeId");

            migrationBuilder.RenameColumn(
                name: "AttendanceType",
                table: "AttendanceDetails",
                newName: "AttendanceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleTypeId",
                table: "Vehicles",
                column: "VehicleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransactionTypeId",
                table: "Transactions",
                column: "TransactionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tours_TourStatusId",
                table: "Tours",
                column: "TourStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Tours_TourTypeId",
                table: "Tours",
                column: "TourTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tours_VehicleTypeId",
                table: "Tours",
                column: "VehicleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCostHistorys_UnitId",
                table: "ServiceCostHistorys",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateTourRequests_MainVehicleId",
                table: "PrivateTourRequests",
                column: "MainVehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateTourRequests_PrivateTourStatusId",
                table: "PrivateTourRequests",
                column: "PrivateTourStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateJoinTourRequests_JoinTourStatusId",
                table: "PrivateJoinTourRequests",
                column: "JoinTourStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderStatusId",
                table: "Orders",
                column: "OrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionQuotations_OptionClassId",
                table: "OptionQuotations",
                column: "OptionClassId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionQuotations_OptionQuotationStatusId",
                table: "OptionQuotations",
                column: "OptionQuotationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_MaterialTypeId",
                table: "Materials",
                column: "MaterialTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRoutes_AttendanceRouteTypeId",
                table: "AttendanceRoutes",
                column: "AttendanceRouteTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceDetails_AttendanceTypeId",
                table: "AttendanceDetails",
                column: "AttendanceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceDetails_AttendanceTypes_AttendanceTypeId",
                table: "AttendanceDetails",
                column: "AttendanceTypeId",
                principalTable: "AttendanceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceRoutes_AttendanceRouteTypes_AttendanceRouteTypeId",
                table: "AttendanceRoutes",
                column: "AttendanceRouteTypeId",
                principalTable: "AttendanceRouteTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_MaterialTypes_MaterialTypeId",
                table: "Materials",
                column: "MaterialTypeId",
                principalTable: "MaterialTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OptionQuotations_OptionClasses_OptionClassId",
                table: "OptionQuotations",
                column: "OptionClassId",
                principalTable: "OptionClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OptionQuotations_OptionQuotationStatuses_OptionQuotationStatusId",
                table: "OptionQuotations",
                column: "OptionQuotationStatusId",
                principalTable: "OptionQuotationStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderStatuses_OrderStatusId",
                table: "Orders",
                column: "OrderStatusId",
                principalTable: "OrderStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateJoinTourRequests_JoinTourStatuses_JoinTourStatusId",
                table: "PrivateJoinTourRequests",
                column: "JoinTourStatusId",
                principalTable: "JoinTourStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateTourRequests_PrivateTourStatuses_PrivateTourStatusId",
                table: "PrivateTourRequests",
                column: "PrivateTourStatusId",
                principalTable: "PrivateTourStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateTourRequests_VehicleTypes_MainVehicleId",
                table: "PrivateTourRequests",
                column: "MainVehicleId",
                principalTable: "VehicleTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCostHistorys_Units_UnitId",
                table: "ServiceCostHistorys",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tours_TourStatuses_TourStatusId",
                table: "Tours",
                column: "TourStatusId",
                principalTable: "TourStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tours_TourTypes_TourTypeId",
                table: "Tours",
                column: "TourTypeId",
                principalTable: "TourTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tours_VehicleTypes_VehicleTypeId",
                table: "Tours",
                column: "VehicleTypeId",
                principalTable: "VehicleTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_TransactionTypes_TransactionTypeId",
                table: "Transactions",
                column: "TransactionTypeId",
                principalTable: "TransactionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_VehicleTypes_VehicleTypeId",
                table: "Vehicles",
                column: "VehicleTypeId",
                principalTable: "VehicleTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceDetails_AttendanceTypes_AttendanceTypeId",
                table: "AttendanceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceRoutes_AttendanceRouteTypes_AttendanceRouteTypeId",
                table: "AttendanceRoutes");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_MaterialTypes_MaterialTypeId",
                table: "Materials");

            migrationBuilder.DropForeignKey(
                name: "FK_OptionQuotations_OptionClasses_OptionClassId",
                table: "OptionQuotations");

            migrationBuilder.DropForeignKey(
                name: "FK_OptionQuotations_OptionQuotationStatuses_OptionQuotationStatusId",
                table: "OptionQuotations");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderStatuses_OrderStatusId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateJoinTourRequests_JoinTourStatuses_JoinTourStatusId",
                table: "PrivateJoinTourRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateTourRequests_PrivateTourStatuses_PrivateTourStatusId",
                table: "PrivateTourRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateTourRequests_VehicleTypes_MainVehicleId",
                table: "PrivateTourRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCostHistorys_Units_UnitId",
                table: "ServiceCostHistorys");

            migrationBuilder.DropForeignKey(
                name: "FK_Tours_TourStatuses_TourStatusId",
                table: "Tours");

            migrationBuilder.DropForeignKey(
                name: "FK_Tours_TourTypes_TourTypeId",
                table: "Tours");

            migrationBuilder.DropForeignKey(
                name: "FK_Tours_VehicleTypes_VehicleTypeId",
                table: "Tours");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_TransactionTypes_TransactionTypeId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_VehicleTypes_VehicleTypeId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_VehicleTypeId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_TransactionTypeId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Tours_TourStatusId",
                table: "Tours");

            migrationBuilder.DropIndex(
                name: "IX_Tours_TourTypeId",
                table: "Tours");

            migrationBuilder.DropIndex(
                name: "IX_Tours_VehicleTypeId",
                table: "Tours");

            migrationBuilder.DropIndex(
                name: "IX_ServiceCostHistorys_UnitId",
                table: "ServiceCostHistorys");

            migrationBuilder.DropIndex(
                name: "IX_PrivateTourRequests_MainVehicleId",
                table: "PrivateTourRequests");

            migrationBuilder.DropIndex(
                name: "IX_PrivateTourRequests_PrivateTourStatusId",
                table: "PrivateTourRequests");

            migrationBuilder.DropIndex(
                name: "IX_PrivateJoinTourRequests_JoinTourStatusId",
                table: "PrivateJoinTourRequests");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderStatusId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OptionQuotations_OptionClassId",
                table: "OptionQuotations");

            migrationBuilder.DropIndex(
                name: "IX_OptionQuotations_OptionQuotationStatusId",
                table: "OptionQuotations");

            migrationBuilder.DropIndex(
                name: "IX_Materials_MaterialTypeId",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_AttendanceRoutes_AttendanceRouteTypeId",
                table: "AttendanceRoutes");

            migrationBuilder.DropIndex(
                name: "IX_AttendanceDetails_AttendanceTypeId",
                table: "AttendanceDetails");

            migrationBuilder.RenameColumn(
                name: "VehicleTypeId",
                table: "Vehicles",
                newName: "VehicleType");

            migrationBuilder.RenameColumn(
                name: "TransactionTypeId",
                table: "Transactions",
                newName: "TransactionType");

            migrationBuilder.RenameColumn(
                name: "VehicleTypeId",
                table: "Tours",
                newName: "TourType");

            migrationBuilder.RenameColumn(
                name: "TourTypeId",
                table: "Tours",
                newName: "TourStatus");

            migrationBuilder.RenameColumn(
                name: "TourStatusId",
                table: "Tours",
                newName: "MainVehicle");

            migrationBuilder.RenameColumn(
                name: "UnitId",
                table: "ServiceCostHistorys",
                newName: "Unit");

            migrationBuilder.RenameColumn(
                name: "PrivateTourStatusId",
                table: "PrivateTourRequests",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "MainVehicleId",
                table: "PrivateTourRequests",
                newName: "MainVehicle");

            migrationBuilder.RenameColumn(
                name: "JoinTourStatusId",
                table: "PrivateJoinTourRequests",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "OrderStatusId",
                table: "Orders",
                newName: "OrderStatus");

            migrationBuilder.RenameColumn(
                name: "OptionQuotationStatusId",
                table: "OptionQuotations",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "OptionClassId",
                table: "OptionQuotations",
                newName: "OptionClass");

            migrationBuilder.RenameColumn(
                name: "MaterialTypeId",
                table: "Materials",
                newName: "MaterialType");

            migrationBuilder.RenameColumn(
                name: "AttendanceRouteTypeId",
                table: "AttendanceRoutes",
                newName: "AttendanceRouteType");

            migrationBuilder.RenameColumn(
                name: "AttendanceTypeId",
                table: "AttendanceDetails",
                newName: "AttendanceType");
        }
    }
}
