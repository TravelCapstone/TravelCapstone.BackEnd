using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class AddNewStatusToPrivateTourRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PrivateTourStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[] { 4, "PLANCREATED" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PrivateTourStatuses",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
