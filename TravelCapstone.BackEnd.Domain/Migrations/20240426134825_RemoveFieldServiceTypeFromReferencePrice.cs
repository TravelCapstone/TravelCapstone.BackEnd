using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class RemoveFieldServiceTypeFromReferencePrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReferenceTransportPrices_ServiceTypes_ServiceTypeId",
                table: "ReferenceTransportPrices");

            migrationBuilder.DropIndex(
                name: "IX_ReferenceTransportPrices_ServiceTypeId",
                table: "ReferenceTransportPrices");

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("0f47b8b8-dfa4-403c-9969-5dec814c1938"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("225758e0-f657-446e-98fd-c974e55d40b8"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("4652f741-3085-45a2-90a8-f3709b96fb68"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("55f6c585-d1fc-4ba0-b6aa-954c081b43fe"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("94fdaf0b-f062-42f9-a816-c0b149a39599"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("a2251a62-5a35-4730-9c28-efd31f256641"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("a537acb2-db0a-41a8-bf1e-3f3b9d4cf063"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("af92daf2-bf7f-4986-8261-e48dfc92a59d"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("b266b810-c9c9-4ba6-b650-6f46bc62f511"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("b64f6c06-4e46-49b1-ad5c-580dada8e999"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("d366d607-c5a2-47e0-bbcd-1048bf41894b"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("d6590997-ee47-4fb2-a6d2-a0664c515fee"));

            migrationBuilder.DropColumn(
                name: "ServiceTypeId",
                table: "ReferenceTransportPrices");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<int>(
                name: "ServiceTypeId",
                table: "ReferenceTransportPrices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "FacilityRatings",
                columns: new[] { "Id", "FacilityTypeId", "RatingId" },
                values: new object[,]
                {
                    { new Guid("0f47b8b8-dfa4-403c-9969-5dec814c1938"), 0, 1 },
                    { new Guid("225758e0-f657-446e-98fd-c974e55d40b8"), 1, 8 },
                    { new Guid("4652f741-3085-45a2-90a8-f3709b96fb68"), 2, 11 },
                    { new Guid("55f6c585-d1fc-4ba0-b6aa-954c081b43fe"), 0, 0 },
                    { new Guid("94fdaf0b-f062-42f9-a816-c0b149a39599"), 1, 5 },
                    { new Guid("a2251a62-5a35-4730-9c28-efd31f256641"), 1, 7 },
                    { new Guid("a537acb2-db0a-41a8-bf1e-3f3b9d4cf063"), 1, 9 },
                    { new Guid("af92daf2-bf7f-4986-8261-e48dfc92a59d"), 0, 3 },
                    { new Guid("b266b810-c9c9-4ba6-b650-6f46bc62f511"), 0, 0 },
                    { new Guid("b64f6c06-4e46-49b1-ad5c-580dada8e999"), 1, 6 },
                    { new Guid("d366d607-c5a2-47e0-bbcd-1048bf41894b"), 0, 4 },
                    { new Guid("d6590997-ee47-4fb2-a6d2-a0664c515fee"), 0, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReferenceTransportPrices_ServiceTypeId",
                table: "ReferenceTransportPrices",
                column: "ServiceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReferenceTransportPrices_ServiceTypes_ServiceTypeId",
                table: "ReferenceTransportPrices",
                column: "ServiceTypeId",
                principalTable: "ServiceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
