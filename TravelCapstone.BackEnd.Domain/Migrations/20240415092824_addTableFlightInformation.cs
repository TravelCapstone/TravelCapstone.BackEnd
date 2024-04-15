using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class addTableFlightInformation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "PrivateTourRequests");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "PrivateTourRequests");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "OptionQuotations");

            migrationBuilder.RenameColumn(
                name: "Total",
                table: "OptionQuotations",
                newName: "MinTotal");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "PrivateTourRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "MainDestinationId",
                table: "PrivateTourRequests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "NumOfDay",
                table: "PrivateTourRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumOfNight",
                table: "PrivateTourRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "MaxTotal",
                table: "OptionQuotations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "FlightInformations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartPoint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndPoint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuotationDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightInformations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlightInformations_QuotationDetails_QuotationDetailId",
                        column: x => x.QuotationDetailId,
                        principalTable: "QuotationDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrivateTourRequests_MainDestinationId",
                table: "PrivateTourRequests",
                column: "MainDestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightInformations_QuotationDetailId",
                table: "FlightInformations",
                column: "QuotationDetailId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateTourRequests_Provinces_MainDestinationId",
                table: "PrivateTourRequests",
                column: "MainDestinationId",
                principalTable: "Provinces",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrivateTourRequests_Provinces_MainDestinationId",
                table: "PrivateTourRequests");

            migrationBuilder.DropTable(
                name: "FlightInformations");

            migrationBuilder.DropIndex(
                name: "IX_PrivateTourRequests_MainDestinationId",
                table: "PrivateTourRequests");

            migrationBuilder.DropColumn(
                name: "MainDestinationId",
                table: "PrivateTourRequests");

            migrationBuilder.DropColumn(
                name: "NumOfDay",
                table: "PrivateTourRequests");

            migrationBuilder.DropColumn(
                name: "NumOfNight",
                table: "PrivateTourRequests");

            migrationBuilder.DropColumn(
                name: "MaxTotal",
                table: "OptionQuotations");

            migrationBuilder.RenameColumn(
                name: "MinTotal",
                table: "OptionQuotations",
                newName: "Total");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "PrivateTourRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "PrivateTourRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PrivateTourRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "OptionQuotations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
