using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class AddAddressPrivatetoureRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StartLocationCommuneId",
                table: "PrivateTourRequests",
                type: "uniqueidentifier",
                nullable: true,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PrivateTourRequests_StartLocationCommuneId",
                table: "PrivateTourRequests",
                column: "StartLocationCommuneId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateTourRequests_Communes_StartLocationCommuneId",
                table: "PrivateTourRequests",
                column: "StartLocationCommuneId",
                principalTable: "Communes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrivateTourRequests_Communes_StartLocationCommuneId",
                table: "PrivateTourRequests");

            migrationBuilder.DropIndex(
                name: "IX_PrivateTourRequests_StartLocationCommuneId",
                table: "PrivateTourRequests");

            migrationBuilder.DropColumn(
                name: "StartLocationCommuneId",
                table: "PrivateTourRequests");
        }
    }
}
