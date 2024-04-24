using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class fixDbBigUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrivateTourRequests_Communes_StartLocationCommuneId",
                table: "PrivateTourRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_QuotationDetails_ServiceRatings_ServiceRatingId",
                table: "QuotationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ReferenceTransportPrices_ServiceRatings_ServiceRatingId",
                table: "ReferenceTransportPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_SellPriceHistorys_Services_ServiceId",
                table: "SellPriceHistorys");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCostHistorys_Services_ServiceId",
                table: "ServiceCostHistorys");

            migrationBuilder.DropForeignKey(
                name: "FK_Tours_AspNetUsers_TourGuideId",
                table: "Tours");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleQuotationDetails_Services_ServiceId",
                table: "VehicleQuotationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleRoutes_ReferenceTransportPrices_ReferenceTransportPriceId",
                table: "VehicleRoutes");

            migrationBuilder.DropTable(
                name: "MainVehicleTours");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "ServiceRatings");

            migrationBuilder.DropIndex(
                name: "IX_Tours_TourGuideId",
                table: "Tours");

            migrationBuilder.DropIndex(
                name: "IX_ServiceCostHistorys_ServiceId",
                table: "ServiceCostHistorys");

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "TourGuideId",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "ServiceCostHistorys");

            migrationBuilder.RenameColumn(
                name: "ServiceId",
                table: "VehicleQuotationDetails",
                newName: "FacilityId");

            migrationBuilder.RenameIndex(
                name: "IX_VehicleQuotationDetails_ServiceId",
                table: "VehicleQuotationDetails",
                newName: "IX_VehicleQuotationDetails_FacilityId");

            migrationBuilder.RenameColumn(
                name: "ServiceId",
                table: "SellPriceHistorys",
                newName: "FacilityServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_SellPriceHistorys_ServiceId",
                table: "SellPriceHistorys",
                newName: "IX_SellPriceHistorys_FacilityServiceId");

            migrationBuilder.RenameColumn(
                name: "ServiceRatingId",
                table: "ReferenceTransportPrices",
                newName: "FacilityRatingId");

            migrationBuilder.RenameIndex(
                name: "IX_ReferenceTransportPrices_ServiceRatingId",
                table: "ReferenceTransportPrices",
                newName: "IX_ReferenceTransportPrices_FacilityRatingId");

            migrationBuilder.RenameColumn(
                name: "ServiceRatingId",
                table: "QuotationDetails",
                newName: "FacilityRatingId");

            migrationBuilder.RenameIndex(
                name: "IX_QuotationDetails_ServiceRatingId",
                table: "QuotationDetails",
                newName: "IX_QuotationDetails_FacilityRatingId");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "PlanServiceCostDetails",
                newName: "QuantityOfChild");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReferenceTransportPriceId",
                table: "VehicleRoutes",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "FacilityServiceId",
                table: "ServiceCostHistorys",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MenuId",
                table: "ServiceCostHistorys",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TransportServiceDetailId",
                table: "ServiceCostHistorys",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServiceTypeId",
                table: "ReferenceTransportPrices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ServiceTypeId",
                table: "QuotationDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "StartLocationCommuneId",
                table: "PrivateTourRequests",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<int>(
                name: "DietaryPreferenceId",
                table: "PrivateTourRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "WishPrice",
                table: "PrivateTourRequests",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "QuantityOfAdult",
                table: "PlanServiceCostDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DietaryPreferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DietaryPreferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DishTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FacilityTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilityTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TourguideAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProvinceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourguideAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourguideAssignments_AspNetUsers_AccountId",
                        column: x => x.AccountId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TourguideAssignments_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransportServiceDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServingQuantity = table.Column<int>(type: "int", nullable: false),
                    VehicleTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportServiceDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportServiceDetails_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalTable: "VehicleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dishes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DishTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dishes_DishTypes_DishTypeId",
                        column: x => x.DishTypeId,
                        principalTable: "DishTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FacilityRatings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacilityTypeId = table.Column<int>(type: "int", nullable: false),
                    RatingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilityRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacilityRatings_FacilityTypes_FacilityTypeId",
                        column: x => x.FacilityTypeId,
                        principalTable: "FacilityTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FacilityRatings_Ratings_RatingId",
                        column: x => x.RatingId,
                        principalTable: "Ratings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TourTourguides",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TourId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TourguideAssignmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourTourguides", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourTourguides_TourguideAssignments_TourguideAssignmentId",
                        column: x => x.TourguideAssignmentId,
                        principalTable: "TourguideAssignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TourTourguides_Tours_TourId",
                        column: x => x.TourId,
                        principalTable: "Tours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Facilities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommunceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceProviderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacilityRatingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Facilities_Communes_CommunceId",
                        column: x => x.CommunceId,
                        principalTable: "Communes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Facilities_FacilityRatings_FacilityRatingId",
                        column: x => x.FacilityRatingId,
                        principalTable: "FacilityRatings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Facilities_ServiceProviders_ServiceProviderId",
                        column: x => x.ServiceProviderId,
                        principalTable: "ServiceProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FacilityServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServingQuantity = table.Column<int>(type: "int", nullable: false),
                    SurchargePercent = table.Column<double>(type: "float", nullable: false),
                    FacilityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceAvailabilityId = table.Column<int>(type: "int", nullable: false),
                    ServiceTypeId = table.Column<int>(type: "int", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilityServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacilityServices_Facilities_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FacilityServices_ServiceAvailabilities_ServiceAvailabilityId",
                        column: x => x.ServiceAvailabilityId,
                        principalTable: "ServiceAvailabilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FacilityServices_ServiceTypes_ServiceTypeId",
                        column: x => x.ServiceTypeId,
                        principalTable: "ServiceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FacilityServices_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DietaryPreferenceId = table.Column<int>(type: "int", nullable: false),
                    FacilityServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menus_DietaryPreferences_DietaryPreferenceId",
                        column: x => x.DietaryPreferenceId,
                        principalTable: "DietaryPreferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Menus_FacilityServices_FacilityServiceId",
                        column: x => x.FacilityServiceId,
                        principalTable: "FacilityServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuDishes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MenuId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DishId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuDishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuDishes_Dishes_DishId",
                        column: x => x.DishId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuDishes_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "FacilityTypes",
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
                table: "Ratings",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "LODGING" },
                    { 1, "HOTEL_TWOSTAR" },
                    { 2, "HOTEL_THREESTAR" },
                    { 3, "HOTEL_FOURSTAR" },
                    { 4, "HOTEL_FIVESTAR" },
                    { 5, "RESTAURENT_CASUAL" },
                    { 6, "RESTAURENT_TWOSTAR" },
                    { 7, "RESTAURENT_THREESTAR" },
                    { 8, "RESTAURENT_FOURSTAR" },
                    { 9, "RESTAURENT_FIVESTAR" },
                    { 10, "RESORT" },
                    { 11, "TOURIST_AREA" }
                });

            migrationBuilder.UpdateData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: 0,
                column: "Name",
                value: "RESTING");

            migrationBuilder.UpdateData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "FOODANDBEVARAGE");

            migrationBuilder.UpdateData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "ENTERTAIMENT");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCostHistorys_FacilityServiceId",
                table: "ServiceCostHistorys",
                column: "FacilityServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCostHistorys_MenuId",
                table: "ServiceCostHistorys",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCostHistorys_TransportServiceDetailId",
                table: "ServiceCostHistorys",
                column: "TransportServiceDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferenceTransportPrices_ServiceTypeId",
                table: "ReferenceTransportPrices",
                column: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationDetails_ServiceTypeId",
                table: "QuotationDetails",
                column: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateTourRequests_DietaryPreferenceId",
                table: "PrivateTourRequests",
                column: "DietaryPreferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_DishTypeId",
                table: "Dishes",
                column: "DishTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Facilities_CommunceId",
                table: "Facilities",
                column: "CommunceId");

            migrationBuilder.CreateIndex(
                name: "IX_Facilities_FacilityRatingId",
                table: "Facilities",
                column: "FacilityRatingId");

            migrationBuilder.CreateIndex(
                name: "IX_Facilities_ServiceProviderId",
                table: "Facilities",
                column: "ServiceProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityRatings_FacilityTypeId",
                table: "FacilityRatings",
                column: "FacilityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityRatings_RatingId",
                table: "FacilityRatings",
                column: "RatingId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityServices_FacilityId",
                table: "FacilityServices",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityServices_ServiceAvailabilityId",
                table: "FacilityServices",
                column: "ServiceAvailabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityServices_ServiceTypeId",
                table: "FacilityServices",
                column: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityServices_UnitId",
                table: "FacilityServices",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuDishes_DishId",
                table: "MenuDishes",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuDishes_MenuId",
                table: "MenuDishes",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_DietaryPreferenceId",
                table: "Menus",
                column: "DietaryPreferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_FacilityServiceId",
                table: "Menus",
                column: "FacilityServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TourguideAssignments_AccountId",
                table: "TourguideAssignments",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TourguideAssignments_ProvinceId",
                table: "TourguideAssignments",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_TourTourguides_TourguideAssignmentId",
                table: "TourTourguides",
                column: "TourguideAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TourTourguides_TourId",
                table: "TourTourguides",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportServiceDetails_VehicleTypeId",
                table: "TransportServiceDetails",
                column: "VehicleTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateTourRequests_Communes_StartLocationCommuneId",
                table: "PrivateTourRequests",
                column: "StartLocationCommuneId",
                principalTable: "Communes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateTourRequests_DietaryPreferences_DietaryPreferenceId",
                table: "PrivateTourRequests",
                column: "DietaryPreferenceId",
                principalTable: "DietaryPreferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuotationDetails_FacilityRatings_FacilityRatingId",
                table: "QuotationDetails",
                column: "FacilityRatingId",
                principalTable: "FacilityRatings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuotationDetails_ServiceTypes_ServiceTypeId",
                table: "QuotationDetails",
                column: "ServiceTypeId",
                principalTable: "ServiceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReferenceTransportPrices_FacilityRatings_FacilityRatingId",
                table: "ReferenceTransportPrices",
                column: "FacilityRatingId",
                principalTable: "FacilityRatings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReferenceTransportPrices_ServiceTypes_ServiceTypeId",
                table: "ReferenceTransportPrices",
                column: "ServiceTypeId",
                principalTable: "ServiceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SellPriceHistorys_FacilityServices_FacilityServiceId",
                table: "SellPriceHistorys",
                column: "FacilityServiceId",
                principalTable: "FacilityServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCostHistorys_FacilityServices_FacilityServiceId",
                table: "ServiceCostHistorys",
                column: "FacilityServiceId",
                principalTable: "FacilityServices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCostHistorys_Menus_MenuId",
                table: "ServiceCostHistorys",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCostHistorys_TransportServiceDetails_TransportServiceDetailId",
                table: "ServiceCostHistorys",
                column: "TransportServiceDetailId",
                principalTable: "TransportServiceDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleQuotationDetails_Facilities_FacilityId",
                table: "VehicleQuotationDetails",
                column: "FacilityId",
                principalTable: "Facilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleRoutes_ReferenceTransportPrices_ReferenceTransportPriceId",
                table: "VehicleRoutes",
                column: "ReferenceTransportPriceId",
                principalTable: "ReferenceTransportPrices",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrivateTourRequests_Communes_StartLocationCommuneId",
                table: "PrivateTourRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateTourRequests_DietaryPreferences_DietaryPreferenceId",
                table: "PrivateTourRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_QuotationDetails_FacilityRatings_FacilityRatingId",
                table: "QuotationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_QuotationDetails_ServiceTypes_ServiceTypeId",
                table: "QuotationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ReferenceTransportPrices_FacilityRatings_FacilityRatingId",
                table: "ReferenceTransportPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_ReferenceTransportPrices_ServiceTypes_ServiceTypeId",
                table: "ReferenceTransportPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_SellPriceHistorys_FacilityServices_FacilityServiceId",
                table: "SellPriceHistorys");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCostHistorys_FacilityServices_FacilityServiceId",
                table: "ServiceCostHistorys");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCostHistorys_Menus_MenuId",
                table: "ServiceCostHistorys");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCostHistorys_TransportServiceDetails_TransportServiceDetailId",
                table: "ServiceCostHistorys");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleQuotationDetails_Facilities_FacilityId",
                table: "VehicleQuotationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleRoutes_ReferenceTransportPrices_ReferenceTransportPriceId",
                table: "VehicleRoutes");

            migrationBuilder.DropTable(
                name: "MenuDishes");

            migrationBuilder.DropTable(
                name: "TourTourguides");

            migrationBuilder.DropTable(
                name: "TransportServiceDetails");

            migrationBuilder.DropTable(
                name: "Dishes");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "TourguideAssignments");

            migrationBuilder.DropTable(
                name: "DishTypes");

            migrationBuilder.DropTable(
                name: "DietaryPreferences");

            migrationBuilder.DropTable(
                name: "FacilityServices");

            migrationBuilder.DropTable(
                name: "Facilities");

            migrationBuilder.DropTable(
                name: "FacilityRatings");

            migrationBuilder.DropTable(
                name: "FacilityTypes");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_ServiceCostHistorys_FacilityServiceId",
                table: "ServiceCostHistorys");

            migrationBuilder.DropIndex(
                name: "IX_ServiceCostHistorys_MenuId",
                table: "ServiceCostHistorys");

            migrationBuilder.DropIndex(
                name: "IX_ServiceCostHistorys_TransportServiceDetailId",
                table: "ServiceCostHistorys");

            migrationBuilder.DropIndex(
                name: "IX_ReferenceTransportPrices_ServiceTypeId",
                table: "ReferenceTransportPrices");

            migrationBuilder.DropIndex(
                name: "IX_QuotationDetails_ServiceTypeId",
                table: "QuotationDetails");

            migrationBuilder.DropIndex(
                name: "IX_PrivateTourRequests_DietaryPreferenceId",
                table: "PrivateTourRequests");

            migrationBuilder.DropColumn(
                name: "FacilityServiceId",
                table: "ServiceCostHistorys");

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "ServiceCostHistorys");

            migrationBuilder.DropColumn(
                name: "TransportServiceDetailId",
                table: "ServiceCostHistorys");

            migrationBuilder.DropColumn(
                name: "ServiceTypeId",
                table: "ReferenceTransportPrices");

            migrationBuilder.DropColumn(
                name: "ServiceTypeId",
                table: "QuotationDetails");

            migrationBuilder.DropColumn(
                name: "DietaryPreferenceId",
                table: "PrivateTourRequests");

            migrationBuilder.DropColumn(
                name: "WishPrice",
                table: "PrivateTourRequests");

            migrationBuilder.DropColumn(
                name: "QuantityOfAdult",
                table: "PlanServiceCostDetails");

            migrationBuilder.RenameColumn(
                name: "FacilityId",
                table: "VehicleQuotationDetails",
                newName: "ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_VehicleQuotationDetails_FacilityId",
                table: "VehicleQuotationDetails",
                newName: "IX_VehicleQuotationDetails_ServiceId");

            migrationBuilder.RenameColumn(
                name: "FacilityServiceId",
                table: "SellPriceHistorys",
                newName: "ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_SellPriceHistorys_FacilityServiceId",
                table: "SellPriceHistorys",
                newName: "IX_SellPriceHistorys_ServiceId");

            migrationBuilder.RenameColumn(
                name: "FacilityRatingId",
                table: "ReferenceTransportPrices",
                newName: "ServiceRatingId");

            migrationBuilder.RenameIndex(
                name: "IX_ReferenceTransportPrices_FacilityRatingId",
                table: "ReferenceTransportPrices",
                newName: "IX_ReferenceTransportPrices_ServiceRatingId");

            migrationBuilder.RenameColumn(
                name: "FacilityRatingId",
                table: "QuotationDetails",
                newName: "ServiceRatingId");

            migrationBuilder.RenameIndex(
                name: "IX_QuotationDetails_FacilityRatingId",
                table: "QuotationDetails",
                newName: "IX_QuotationDetails_ServiceRatingId");

            migrationBuilder.RenameColumn(
                name: "QuantityOfChild",
                table: "PlanServiceCostDetails",
                newName: "Quantity");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReferenceTransportPriceId",
                table: "VehicleRoutes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TourGuideId",
                table: "Tours",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceId",
                table: "ServiceCostHistorys",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "StartLocationCommuneId",
                table: "PrivateTourRequests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "MainVehicleTours",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainVehicleTours", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceRatings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceTypeId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceRatings_ServiceTypes_ServiceTypeId",
                        column: x => x.ServiceTypeId,
                        principalTable: "ServiceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommunceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceAvailabilityId = table.Column<int>(type: "int", nullable: false),
                    ServiceProviderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceRatingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServingQuantity = table.Column<int>(type: "int", nullable: false),
                    SurchargePercent = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_Communes_CommunceId",
                        column: x => x.CommunceId,
                        principalTable: "Communes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Services_ServiceAvailabilities_ServiceAvailabilityId",
                        column: x => x.ServiceAvailabilityId,
                        principalTable: "ServiceAvailabilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Services_ServiceProviders_ServiceProviderId",
                        column: x => x.ServiceProviderId,
                        principalTable: "ServiceProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Services_ServiceRatings_ServiceRatingId",
                        column: x => x.ServiceRatingId,
                        principalTable: "ServiceRatings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Services_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: 0,
                column: "Name",
                value: "HOTEL");

            migrationBuilder.UpdateData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "RESTAURANTS");

            migrationBuilder.UpdateData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "ENTERTAINMENT");

            migrationBuilder.InsertData(
                table: "ServiceTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 3, "VEHICLE_SUPPLY" },
                    { 4, "AIR_TICKET_SUPPLY" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tours_TourGuideId",
                table: "Tours",
                column: "TourGuideId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCostHistorys_ServiceId",
                table: "ServiceCostHistorys",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRatings_ServiceTypeId",
                table: "ServiceRatings",
                column: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_CommunceId",
                table: "Services",
                column: "CommunceId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServiceAvailabilityId",
                table: "Services",
                column: "ServiceAvailabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServiceProviderId",
                table: "Services",
                column: "ServiceProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServiceRatingId",
                table: "Services",
                column: "ServiceRatingId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_UnitId",
                table: "Services",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateTourRequests_Communes_StartLocationCommuneId",
                table: "PrivateTourRequests",
                column: "StartLocationCommuneId",
                principalTable: "Communes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuotationDetails_ServiceRatings_ServiceRatingId",
                table: "QuotationDetails",
                column: "ServiceRatingId",
                principalTable: "ServiceRatings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReferenceTransportPrices_ServiceRatings_ServiceRatingId",
                table: "ReferenceTransportPrices",
                column: "ServiceRatingId",
                principalTable: "ServiceRatings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SellPriceHistorys_Services_ServiceId",
                table: "SellPriceHistorys",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCostHistorys_Services_ServiceId",
                table: "ServiceCostHistorys",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tours_AspNetUsers_TourGuideId",
                table: "Tours",
                column: "TourGuideId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleQuotationDetails_Services_ServiceId",
                table: "VehicleQuotationDetails",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleRoutes_ReferenceTransportPrices_ReferenceTransportPriceId",
                table: "VehicleRoutes",
                column: "ReferenceTransportPriceId",
                principalTable: "ReferenceTransportPrices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
