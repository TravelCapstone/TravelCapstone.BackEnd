using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class DayplanToTourInMaterial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materials_DayPlans_DayPlanId",
                table: "Materials");

            migrationBuilder.RenameColumn(
                name: "DayPlanId",
                table: "Materials",
                newName: "TourId");

            migrationBuilder.RenameIndex(
                name: "IX_Materials_DayPlanId",
                table: "Materials",
                newName: "IX_Materials_TourId");

            migrationBuilder.AddColumn<int>(
                name: "MealPerDay",
                table: "QuotationDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "QuotationDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ServingQuantity",
                table: "QuotationDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Tours_TourId",
                table: "Materials",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Tours_TourId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "MealPerDay",
                table: "QuotationDetails");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "QuotationDetails");

            migrationBuilder.DropColumn(
                name: "ServingQuantity",
                table: "QuotationDetails");

            migrationBuilder.RenameColumn(
                name: "TourId",
                table: "Materials",
                newName: "DayPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_Materials_TourId",
                table: "Materials",
                newName: "IX_Materials_DayPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_DayPlans_DayPlanId",
                table: "Materials",
                column: "DayPlanId",
                principalTable: "DayPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
