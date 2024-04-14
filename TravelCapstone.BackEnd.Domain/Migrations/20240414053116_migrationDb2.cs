using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class migrationDb2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuotationDetails_SellPriceHistorys_SellPriceHistoryId",
                table: "QuotationDetails");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Services");

            migrationBuilder.RenameColumn(
                name: "SellPriceHistoryId",
                table: "QuotationDetails",
                newName: "ServiceRatingId");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "QuotationDetails",
                newName: "QuantityOfChild");

            migrationBuilder.RenameIndex(
                name: "IX_QuotationDetails_SellPriceHistoryId",
                table: "QuotationDetails",
                newName: "IX_QuotationDetails_ServiceRatingId");

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceRatingId",
                table: "Services",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "MaxPrice",
                table: "QuotationDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinPrice",
                table: "QuotationDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuantityOfAdult",
                table: "QuotationDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AttendanceRouteTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceRouteTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JoinTourStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JoinTourStatuses", x => x.Id);
                });

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
                name: "MaterialTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OptionClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionClasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OptionQuotationStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionQuotationStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrivateTourStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateTourStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TourStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TourTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleTypes", x => x.Id);
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

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServiceRatingId",
                table: "Services",
                column: "ServiceRatingId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRatings_ServiceTypeId",
                table: "ServiceRatings",
                column: "ServiceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuotationDetails_ServiceRatings_ServiceRatingId",
                table: "QuotationDetails",
                column: "ServiceRatingId",
                principalTable: "ServiceRatings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ServiceRatings_ServiceRatingId",
                table: "Services",
                column: "ServiceRatingId",
                principalTable: "ServiceRatings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuotationDetails_ServiceRatings_ServiceRatingId",
                table: "QuotationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_ServiceRatings_ServiceRatingId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "AttendanceRouteTypes");

            migrationBuilder.DropTable(
                name: "AttendanceTypes");

            migrationBuilder.DropTable(
                name: "JoinTourStatuses");

            migrationBuilder.DropTable(
                name: "MainVehicleTours");

            migrationBuilder.DropTable(
                name: "MaterialTypes");

            migrationBuilder.DropTable(
                name: "OptionClasses");

            migrationBuilder.DropTable(
                name: "OptionQuotationStatuses");

            migrationBuilder.DropTable(
                name: "OrderStatuses");

            migrationBuilder.DropTable(
                name: "PrivateTourStatuses");

            migrationBuilder.DropTable(
                name: "ServiceRatings");

            migrationBuilder.DropTable(
                name: "TourStatuses");

            migrationBuilder.DropTable(
                name: "TourTypes");

            migrationBuilder.DropTable(
                name: "TransactionTypes");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "VehicleTypes");

            migrationBuilder.DropTable(
                name: "ServiceTypes");

            migrationBuilder.DropIndex(
                name: "IX_Services_ServiceRatingId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ServiceRatingId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "MaxPrice",
                table: "QuotationDetails");

            migrationBuilder.DropColumn(
                name: "MinPrice",
                table: "QuotationDetails");

            migrationBuilder.DropColumn(
                name: "QuantityOfAdult",
                table: "QuotationDetails");

            migrationBuilder.RenameColumn(
                name: "ServiceRatingId",
                table: "QuotationDetails",
                newName: "SellPriceHistoryId");

            migrationBuilder.RenameColumn(
                name: "QuantityOfChild",
                table: "QuotationDetails",
                newName: "Quantity");

            migrationBuilder.RenameIndex(
                name: "IX_QuotationDetails_ServiceRatingId",
                table: "QuotationDetails",
                newName: "IX_QuotationDetails_SellPriceHistoryId");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Services",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_QuotationDetails_SellPriceHistorys_SellPriceHistoryId",
                table: "QuotationDetails",
                column: "SellPriceHistoryId",
                principalTable: "SellPriceHistorys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
