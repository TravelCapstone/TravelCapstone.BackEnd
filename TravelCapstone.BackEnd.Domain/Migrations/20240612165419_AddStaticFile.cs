using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class AddStaticFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StaticFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FacilityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FacilityServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TourId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaticFiles_Facilities_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facilities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StaticFiles_FacilityServices_FacilityServiceId",
                        column: x => x.FacilityServiceId,
                        principalTable: "FacilityServices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StaticFiles_Tours_TourId",
                        column: x => x.TourId,
                        principalTable: "Tours",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StaticFiles_FacilityId",
                table: "StaticFiles",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_StaticFiles_FacilityServiceId",
                table: "StaticFiles",
                column: "FacilityServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_StaticFiles_TourId",
                table: "StaticFiles",
                column: "TourId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StaticFiles");
        }
    }
}
