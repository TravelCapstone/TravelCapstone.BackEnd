using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class MakeRatingNullableForQuotationDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuotationDetails_FacilityRatings_FacilityRatingId",
                table: "QuotationDetails");

            migrationBuilder.AlterColumn<Guid>(
                name: "FacilityRatingId",
                table: "QuotationDetails",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_QuotationDetails_FacilityRatings_FacilityRatingId",
                table: "QuotationDetails",
                column: "FacilityRatingId",
                principalTable: "FacilityRatings",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuotationDetails_FacilityRatings_FacilityRatingId",
                table: "QuotationDetails");

            migrationBuilder.AlterColumn<Guid>(
                name: "FacilityRatingId",
                table: "QuotationDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_QuotationDetails_FacilityRatings_FacilityRatingId",
                table: "QuotationDetails",
                column: "FacilityRatingId",
                principalTable: "FacilityRatings",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
