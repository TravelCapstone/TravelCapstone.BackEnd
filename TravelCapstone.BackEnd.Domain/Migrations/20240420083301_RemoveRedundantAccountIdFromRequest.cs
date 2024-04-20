using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class RemoveRedundantAccountIdFromRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrivateTourRequests_AspNetUsers_AccountId",
                table: "PrivateTourRequests");

            migrationBuilder.DropIndex(
                name: "IX_PrivateTourRequests_AccountId",
                table: "PrivateTourRequests");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "PrivateTourRequests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountId",
                table: "PrivateTourRequests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrivateTourRequests_AccountId",
                table: "PrivateTourRequests",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateTourRequests_AspNetUsers_AccountId",
                table: "PrivateTourRequests",
                column: "AccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
