using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class AdjustFamilyDataInRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomQuantityDetails_PrivateTourRequests_PrivateTourRequestId",
                table: "RoomQuantityDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomQuantityDetails",
                table: "RoomQuantityDetails");

            migrationBuilder.RenameTable(
                name: "RoomQuantityDetails",
                newName: "FamilyDetailRequests");

            migrationBuilder.RenameColumn(
                name: "TotalRoom",
                table: "FamilyDetailRequests",
                newName: "TotalFamily");

            migrationBuilder.RenameColumn(
                name: "QuantityPerRoom",
                table: "FamilyDetailRequests",
                newName: "NumOfChildren");

            migrationBuilder.RenameIndex(
                name: "IX_RoomQuantityDetails_PrivateTourRequestId",
                table: "FamilyDetailRequests",
                newName: "IX_FamilyDetailRequests_PrivateTourRequestId");

            migrationBuilder.AddColumn<int>(
                name: "NumOfFamily",
                table: "PrivateTourRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumOfSingleFemale",
                table: "PrivateTourRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumOfSingleMale",
                table: "PrivateTourRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumOfAdult",
                table: "FamilyDetailRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FamilyDetailRequests",
                table: "FamilyDetailRequests",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyDetailRequests_PrivateTourRequests_PrivateTourRequestId",
                table: "FamilyDetailRequests",
                column: "PrivateTourRequestId",
                principalTable: "PrivateTourRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FamilyDetailRequests_PrivateTourRequests_PrivateTourRequestId",
                table: "FamilyDetailRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FamilyDetailRequests",
                table: "FamilyDetailRequests");

            migrationBuilder.DropColumn(
                name: "NumOfFamily",
                table: "PrivateTourRequests");

            migrationBuilder.DropColumn(
                name: "NumOfSingleFemale",
                table: "PrivateTourRequests");

            migrationBuilder.DropColumn(
                name: "NumOfSingleMale",
                table: "PrivateTourRequests");

            migrationBuilder.DropColumn(
                name: "NumOfAdult",
                table: "FamilyDetailRequests");

            migrationBuilder.RenameTable(
                name: "FamilyDetailRequests",
                newName: "RoomQuantityDetails");

            migrationBuilder.RenameColumn(
                name: "TotalFamily",
                table: "RoomQuantityDetails",
                newName: "TotalRoom");

            migrationBuilder.RenameColumn(
                name: "NumOfChildren",
                table: "RoomQuantityDetails",
                newName: "QuantityPerRoom");

            migrationBuilder.RenameIndex(
                name: "IX_FamilyDetailRequests_PrivateTourRequestId",
                table: "RoomQuantityDetails",
                newName: "IX_RoomQuantityDetails_PrivateTourRequestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomQuantityDetails",
                table: "RoomQuantityDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomQuantityDetails_PrivateTourRequests_PrivateTourRequestId",
                table: "RoomQuantityDetails",
                column: "PrivateTourRequestId",
                principalTable: "PrivateTourRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
