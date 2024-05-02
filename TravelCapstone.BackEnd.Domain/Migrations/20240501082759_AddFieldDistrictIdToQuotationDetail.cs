using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class AddFieldDistrictIdToQuotationDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DistrictId",
                table: "QuotationDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_QuotationDetails_DistrictId",
                table: "QuotationDetails",
                column: "DistrictId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuotationDetails_Districts_DistrictId",
                table: "QuotationDetails",
                column: "DistrictId",
                principalTable: "Districts",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuotationDetails_Districts_DistrictId",
                table: "QuotationDetails");

            migrationBuilder.DropIndex(
                name: "IX_QuotationDetails_DistrictId",
                table: "QuotationDetails");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "QuotationDetails");
        }
    }
}
