using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class updatetravelcompanion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TravelCompanions_AspNetUsers_AccountId",
                table: "TravelCompanions");

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "TravelCompanions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_TravelCompanions_AspNetUsers_AccountId",
                table: "TravelCompanions",
                column: "AccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TravelCompanions_AspNetUsers_AccountId",
                table: "TravelCompanions");

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "TravelCompanions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TravelCompanions_AspNetUsers_AccountId",
                table: "TravelCompanions",
                column: "AccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}