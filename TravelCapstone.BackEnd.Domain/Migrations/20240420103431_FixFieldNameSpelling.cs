using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class FixFieldNameSpelling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecommnendedTourUrl",
                table: "PrivateTourRequests",
                newName: "RecommendedTourUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecommendedTourUrl",
                table: "PrivateTourRequests",
                newName: "RecommnendedTourUrl");
        }
    }
}
