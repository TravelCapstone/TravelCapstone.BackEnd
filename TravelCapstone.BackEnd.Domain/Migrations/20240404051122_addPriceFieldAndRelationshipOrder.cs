using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class addPriceFieldAndRelationshipOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Tours_TourId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Tours",
                newName: "TotalPrice");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "ServiceCostHistorys",
                newName: "PricePerChild");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "SellPriceHistorys",
                newName: "PricePerChild");

            migrationBuilder.RenameColumn(
                name: "isEnterprise",
                table: "PrivateTourRequests",
                newName: "IsEnterprise");

            migrationBuilder.AddColumn<double>(
                name: "PricePerAdult",
                table: "Tours",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PricePerChild",
                table: "Tours",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PricePerAdult",
                table: "ServiceCostHistorys",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "MOQ",
                table: "SellPriceHistorys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "PricePerAdult",
                table: "SellPriceHistorys",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TourId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "PrivateTourRequestId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PrivateTourRequestId",
                table: "Orders",
                column: "PrivateTourRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_PrivateTourRequests_PrivateTourRequestId",
                table: "Orders",
                column: "PrivateTourRequestId",
                principalTable: "PrivateTourRequests",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Tours_TourId",
                table: "Orders",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_PrivateTourRequests_PrivateTourRequestId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Tours_TourId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PrivateTourRequestId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PricePerAdult",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "PricePerChild",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "PricePerAdult",
                table: "ServiceCostHistorys");

            migrationBuilder.DropColumn(
                name: "MOQ",
                table: "SellPriceHistorys");

            migrationBuilder.DropColumn(
                name: "PricePerAdult",
                table: "SellPriceHistorys");

            migrationBuilder.DropColumn(
                name: "PrivateTourRequestId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "Tours",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "PricePerChild",
                table: "ServiceCostHistorys",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "PricePerChild",
                table: "SellPriceHistorys",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "IsEnterprise",
                table: "PrivateTourRequests",
                newName: "isEnterprise");

            migrationBuilder.AlterColumn<Guid>(
                name: "TourId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Tours_TourId",
                table: "Orders",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
