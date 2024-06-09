using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class AddFees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomEvent",
                table: "OptionEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Assurances",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ManagementFeeTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagementFeeTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoomQuantityDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuantityPerRoom = table.Column<int>(type: "int", nullable: false),
                    TotalRoom = table.Column<int>(type: "int", nullable: false),
                    PrivateTourRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomQuantityDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomQuantityDetails_PrivateTourRequests_PrivateTourRequestId",
                        column: x => x.PrivateTourRequestId,
                        principalTable: "PrivateTourRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ManagementFeeReferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Moq = table.Column<int>(type: "int", nullable: false),
                    MinFee = table.Column<double>(type: "float", nullable: false),
                    MaxFee = table.Column<double>(type: "float", nullable: false),
                    ManagementFeeTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagementFeeReferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ManagementFeeReferences_ManagementFeeTypes_ManagementFeeTypeId",
                        column: x => x.ManagementFeeTypeId,
                        principalTable: "ManagementFeeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.InsertData(
                table: "ManagementFeeTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "ORGANIZATION_COST" },
                    { 1, "CONTINGENCY_FEE" },
                    { 2, "ESCORT_FEE" },
                    { 3, "OPERATING_FEE" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ManagementFeeReferences_ManagementFeeTypeId",
                table: "ManagementFeeReferences",
                column: "ManagementFeeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomQuantityDetails_PrivateTourRequestId",
                table: "RoomQuantityDetails",
                column: "PrivateTourRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManagementFeeReferences");

            migrationBuilder.DropTable(
                name: "RoomQuantityDetails");

            migrationBuilder.DropTable(
                name: "ManagementFeeTypes");

            migrationBuilder.DropColumn(
                name: "CustomEvent",
                table: "OptionEvents");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Assurances");
        }
    }
}
