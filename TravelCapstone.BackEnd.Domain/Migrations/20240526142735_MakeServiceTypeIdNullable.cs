using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class MakeServiceTypeIdNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuotationDetails_ServiceTypes_ServiceTypeId",
                table: "QuotationDetails");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceTypeId",
                table: "QuotationDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_QuotationDetails_ServiceTypes_ServiceTypeId",
                table: "QuotationDetails",
                column: "ServiceTypeId",
                principalTable: "ServiceTypes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuotationDetails_ServiceTypes_ServiceTypeId",
                table: "QuotationDetails");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceTypeId",
                table: "QuotationDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_QuotationDetails_ServiceTypes_ServiceTypeId",
                table: "QuotationDetails",
                column: "ServiceTypeId",
                principalTable: "ServiceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
