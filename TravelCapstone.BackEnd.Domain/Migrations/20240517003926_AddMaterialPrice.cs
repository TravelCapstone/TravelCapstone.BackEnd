using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class AddMaterialPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaterialAssignments_Materials_MaterialId",
                table: "MaterialAssignments");

            migrationBuilder.RenameColumn(
                name: "MaterialId",
                table: "MaterialAssignments",
                newName: "MaterialPriceHistoryId");

            migrationBuilder.RenameIndex(
                name: "IX_MaterialAssignments_MaterialId",
                table: "MaterialAssignments",
                newName: "IX_MaterialAssignments_MaterialPriceHistoryId");

            migrationBuilder.AddColumn<double>(
                name: "ContingencyFee",
                table: "Tours",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "EscortFee",
                table: "Tours",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OperatingFee",
                table: "Tours",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OrganizationCost",
                table: "Tours",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "MaterialPriceHistoryId",
                table: "QuotationDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MaterialPriceHistoryId",
                table: "PlanServiceCostDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ContingencyFee",
                table: "OptionQuotations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "EscortFee",
                table: "OptionQuotations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OperatingFee",
                table: "OptionQuotations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OrganizationCost",
                table: "OptionQuotations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Events",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "OptionId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PerPerson",
                table: "EventDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "MaterialPriceHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialPriceHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialPriceHistories_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "TourguideQuotationDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsMainTourGuide = table.Column<bool>(type: "bit", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ProvinceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourguideQuotationDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourguideQuotationDetails_OptionQuotations_OptionId",
                        column: x => x.OptionId,
                        principalTable: "OptionQuotations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TourguideQuotationDetails_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuotationDetails_MaterialPriceHistoryId",
                table: "QuotationDetails",
                column: "MaterialPriceHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanServiceCostDetails_MaterialPriceHistoryId",
                table: "PlanServiceCostDetails",
                column: "MaterialPriceHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_OptionId",
                table: "Events",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialPriceHistories_MaterialId",
                table: "MaterialPriceHistories",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_TourguideQuotationDetails_OptionId",
                table: "TourguideQuotationDetails",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_TourguideQuotationDetails_ProvinceId",
                table: "TourguideQuotationDetails",
                column: "ProvinceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_OptionQuotations_OptionId",
                table: "Events",
                column: "OptionId",
                principalTable: "OptionQuotations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialAssignments_MaterialPriceHistories_MaterialPriceHistoryId",
                table: "MaterialAssignments",
                column: "MaterialPriceHistoryId",
                principalTable: "MaterialPriceHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanServiceCostDetails_MaterialPriceHistories_MaterialPriceHistoryId",
                table: "PlanServiceCostDetails",
                column: "MaterialPriceHistoryId",
                principalTable: "MaterialPriceHistories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuotationDetails_MaterialPriceHistories_MaterialPriceHistoryId",
                table: "QuotationDetails",
                column: "MaterialPriceHistoryId",
                principalTable: "MaterialPriceHistories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_OptionQuotations_OptionId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_MaterialAssignments_MaterialPriceHistories_MaterialPriceHistoryId",
                table: "MaterialAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanServiceCostDetails_MaterialPriceHistories_MaterialPriceHistoryId",
                table: "PlanServiceCostDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_QuotationDetails_MaterialPriceHistories_MaterialPriceHistoryId",
                table: "QuotationDetails");

            migrationBuilder.DropTable(
                name: "MaterialPriceHistories");

            migrationBuilder.DropTable(
                name: "TourguideQuotationDetails");

            migrationBuilder.DropIndex(
                name: "IX_QuotationDetails_MaterialPriceHistoryId",
                table: "QuotationDetails");

            migrationBuilder.DropIndex(
                name: "IX_PlanServiceCostDetails_MaterialPriceHistoryId",
                table: "PlanServiceCostDetails");

            migrationBuilder.DropIndex(
                name: "IX_Events_OptionId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ContingencyFee",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "EscortFee",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "OperatingFee",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "OrganizationCost",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "MaterialPriceHistoryId",
                table: "QuotationDetails");

            migrationBuilder.DropColumn(
                name: "MaterialPriceHistoryId",
                table: "PlanServiceCostDetails");

            migrationBuilder.DropColumn(
                name: "ContingencyFee",
                table: "OptionQuotations");

            migrationBuilder.DropColumn(
                name: "EscortFee",
                table: "OptionQuotations");

            migrationBuilder.DropColumn(
                name: "OperatingFee",
                table: "OptionQuotations");

            migrationBuilder.DropColumn(
                name: "OrganizationCost",
                table: "OptionQuotations");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "OptionId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "PerPerson",
                table: "EventDetails");

            migrationBuilder.RenameColumn(
                name: "MaterialPriceHistoryId",
                table: "MaterialAssignments",
                newName: "MaterialId");

            migrationBuilder.RenameIndex(
                name: "IX_MaterialAssignments_MaterialPriceHistoryId",
                table: "MaterialAssignments",
                newName: "IX_MaterialAssignments_MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialAssignments_Materials_MaterialId",
                table: "MaterialAssignments",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
