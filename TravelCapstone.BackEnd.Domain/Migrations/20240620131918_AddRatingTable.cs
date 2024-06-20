using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class AddRatingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TourRatingId",
                table: "StaticFiles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TourRatings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TourId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourRatings_Tours_TourId",
                        column: x => x.TourId,
                        principalTable: "Tours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StaticFiles_TourRatingId",
                table: "StaticFiles",
                column: "TourRatingId");

            migrationBuilder.CreateIndex(
                name: "IX_TourRatings_TourId",
                table: "TourRatings",
                column: "TourId");

            migrationBuilder.AddForeignKey(
                name: "FK_StaticFiles_TourRatings_TourRatingId",
                table: "StaticFiles",
                column: "TourRatingId",
                principalTable: "TourRatings",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaticFiles_TourRatings_TourRatingId",
                table: "StaticFiles");

            migrationBuilder.DropTable(
                name: "TourRatings");

            migrationBuilder.DropIndex(
                name: "IX_StaticFiles_TourRatingId",
                table: "StaticFiles");

            migrationBuilder.DropColumn(
                name: "TourRatingId",
                table: "StaticFiles");
        }
    }
}
