using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class AddGeneratedTourIdToPrivateTourRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GeneratedTourId",
                table: "PrivateTourRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrivateTourRequests_GeneratedTourId",
                table: "PrivateTourRequests",
                column: "GeneratedTourId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateTourRequests_Tours_GeneratedTourId",
                table: "PrivateTourRequests",
                column: "GeneratedTourId",
                principalTable: "Tours",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrivateTourRequests_Tours_GeneratedTourId",
                table: "PrivateTourRequests");

            migrationBuilder.DropIndex(
                name: "IX_PrivateTourRequests_GeneratedTourId",
                table: "PrivateTourRequests");

            migrationBuilder.DropColumn(
                name: "GeneratedTourId",
                table: "PrivateTourRequests");
        }
    }
}
