using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class AddFieldAddressToRequestedLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "RequestedLocations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "RequestedLocations");
        }
    }
}
