using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class AddFieldToReferencePrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProviderName",
                table: "ReferenceTransportPrices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceRatingId",
                table: "ReferenceTransportPrices",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ReferenceTransportPrices_ServiceRatingId",
                table: "ReferenceTransportPrices",
                column: "ServiceRatingId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReferenceTransportPrices_ServiceRatings_ServiceRatingId",
                table: "ReferenceTransportPrices",
                column: "ServiceRatingId",
                principalTable: "ServiceRatings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReferenceTransportPrices_ServiceRatings_ServiceRatingId",
                table: "ReferenceTransportPrices");

            migrationBuilder.DropIndex(
                name: "IX_ReferenceTransportPrices_ServiceRatingId",
                table: "ReferenceTransportPrices");

            migrationBuilder.DropColumn(
                name: "ProviderName",
                table: "ReferenceTransportPrices");

            migrationBuilder.DropColumn(
                name: "ServiceRatingId",
                table: "ReferenceTransportPrices");
        }
    }
}
