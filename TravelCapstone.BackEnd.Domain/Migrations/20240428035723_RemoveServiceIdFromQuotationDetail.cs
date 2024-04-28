using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class RemoveServiceIdFromQuotationDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleQuotationDetails_Facilities_FacilityId",
                table: "VehicleQuotationDetails");

            migrationBuilder.DropIndex(
                name: "IX_VehicleQuotationDetails_FacilityId",
                table: "VehicleQuotationDetails");

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("056c02ac-e4b0-4345-87e8-650fed4479d1"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("10f2342e-924e-4397-83c6-bdd7ad68ada2"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("14da0c8c-8bca-462c-99ca-d901ee5732b7"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("384fefc7-f496-42a1-a5cd-bc0e39558bef"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("3e0a5920-9a4c-4897-9047-e91318a42c20"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("4aa36e58-7117-45e1-8c6d-a792e85fddbc"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("63716fa3-dfbe-41e4-8662-6553c82681af"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("834d96c3-89d5-4fb7-9c88-0277c5a4f3f9"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("a6bc086d-744e-4d3d-a3d3-a55ffc8a8c5b"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("afc4f51f-c024-4c97-a92f-01f3513e8b8d"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("b6f6e5c2-7735-4866-81b7-0b8bb2ab6479"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("e7298609-9d21-47c4-8378-b31bd07d4e88"));

            migrationBuilder.DropColumn(
                name: "FacilityId",
                table: "VehicleQuotationDetails");

            migrationBuilder.InsertData(
                table: "FacilityRatings",
                columns: new[] { "Id", "FacilityTypeId", "RatingId" },
                values: new object[,]
                {
                    { new Guid("00bd483f-5e2d-4b2b-8409-dfa90ba9dbaf"), 0, 0 },
                    { new Guid("0862d814-b6b0-4595-8d07-ef40437c936b"), 0, 0 },
                    { new Guid("0c66e0d9-a06f-4e0b-8edc-7bf874ce5acf"), 1, 5 },
                    { new Guid("2d679fc9-b6bf-435e-a91f-41e3e9e2e2aa"), 0, 2 },
                    { new Guid("3101b2f9-e74d-4736-88f7-2bc6abcba4cb"), 1, 7 },
                    { new Guid("3cca3d32-ceec-491a-b57f-bf5601c7cb5d"), 0, 3 },
                    { new Guid("4d9a2a27-429b-4304-8be8-0fbbdc777de4"), 0, 1 },
                    { new Guid("69ec97db-2c76-4a45-8774-45d0e9691b0f"), 1, 8 },
                    { new Guid("a201d464-99af-4d14-b2c2-dd532aefc3e3"), 1, 9 },
                    { new Guid("d2225937-fd05-4f9a-aefa-a52f5d431300"), 1, 6 },
                    { new Guid("d4725d36-8e6e-4add-b90b-3646080ab23b"), 0, 4 },
                    { new Guid("ff2c86d7-8b6c-454d-af14-d429d2404fe1"), 2, 11 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("00bd483f-5e2d-4b2b-8409-dfa90ba9dbaf"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("0862d814-b6b0-4595-8d07-ef40437c936b"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("0c66e0d9-a06f-4e0b-8edc-7bf874ce5acf"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("2d679fc9-b6bf-435e-a91f-41e3e9e2e2aa"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("3101b2f9-e74d-4736-88f7-2bc6abcba4cb"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("3cca3d32-ceec-491a-b57f-bf5601c7cb5d"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("4d9a2a27-429b-4304-8be8-0fbbdc777de4"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("69ec97db-2c76-4a45-8774-45d0e9691b0f"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("a201d464-99af-4d14-b2c2-dd532aefc3e3"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("d2225937-fd05-4f9a-aefa-a52f5d431300"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("d4725d36-8e6e-4add-b90b-3646080ab23b"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("ff2c86d7-8b6c-454d-af14-d429d2404fe1"));

            migrationBuilder.AddColumn<Guid>(
                name: "FacilityId",
                table: "VehicleQuotationDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "FacilityRatings",
                columns: new[] { "Id", "FacilityTypeId", "RatingId" },
                values: new object[,]
                {
                    { new Guid("056c02ac-e4b0-4345-87e8-650fed4479d1"), 0, 2 },
                    { new Guid("10f2342e-924e-4397-83c6-bdd7ad68ada2"), 0, 0 },
                    { new Guid("14da0c8c-8bca-462c-99ca-d901ee5732b7"), 1, 8 },
                    { new Guid("384fefc7-f496-42a1-a5cd-bc0e39558bef"), 0, 4 },
                    { new Guid("3e0a5920-9a4c-4897-9047-e91318a42c20"), 0, 3 },
                    { new Guid("4aa36e58-7117-45e1-8c6d-a792e85fddbc"), 1, 6 },
                    { new Guid("63716fa3-dfbe-41e4-8662-6553c82681af"), 1, 9 },
                    { new Guid("834d96c3-89d5-4fb7-9c88-0277c5a4f3f9"), 1, 5 },
                    { new Guid("a6bc086d-744e-4d3d-a3d3-a55ffc8a8c5b"), 2, 11 },
                    { new Guid("afc4f51f-c024-4c97-a92f-01f3513e8b8d"), 0, 0 },
                    { new Guid("b6f6e5c2-7735-4866-81b7-0b8bb2ab6479"), 1, 7 },
                    { new Guid("e7298609-9d21-47c4-8378-b31bd07d4e88"), 0, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleQuotationDetails_FacilityId",
                table: "VehicleQuotationDetails",
                column: "FacilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleQuotationDetails_Facilities_FacilityId",
                table: "VehicleQuotationDetails",
                column: "FacilityId",
                principalTable: "Facilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
