using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class addFieldSellPrice2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("2082d4e0-3c81-4f09-a1ee-71c9fbe68695"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("220c785d-85dc-47f9-bd71-84527244ea26"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("4db6fc84-4f56-41be-b451-a90175cbb330"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("90260827-f4a3-4e51-b663-e1e3a04319ee"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("98deef3f-1f0c-42b5-8f6e-691e42599055"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("a3cec749-e989-4dfc-a951-01ae5968643e"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("bb0ce269-6f65-4ad9-a78c-e2d875a3e615"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("c3f8197d-dc54-42d3-a7ac-03b7a54f149f"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("c9140b0f-aa3d-4356-acc6-1d7be690ab69"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("e54995b3-4356-4d8e-9a85-3ea84708b622"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("eaf29dae-91b6-4937-bf98-73dea569608d"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("fb360e5e-00c7-4590-b382-5c2488436f27"));

            migrationBuilder.InsertData(
                table: "FacilityRatings",
                columns: new[] { "Id", "FacilityTypeId", "RatingId" },
                values: new object[,]
                {
                    { new Guid("3266708a-0bd3-4fb2-84fd-b4e826ca2516"), 0, 0 },
                    { new Guid("4b07110d-aed3-4ecd-9158-80376b25c351"), 1, 6 },
                    { new Guid("5234a0c6-d8a8-4f00-abf7-bfe003c9c079"), 0, 2 },
                    { new Guid("5a4ccdaa-1553-420c-9810-30fe3543fb37"), 0, 1 },
                    { new Guid("66a197ae-bba9-4693-a019-317dfc7ae08a"), 1, 7 },
                    { new Guid("7188600d-7729-443b-9293-576c1809f4c0"), 2, 11 },
                    { new Guid("9696a0c1-9856-49d3-bf82-067e66a5e0fc"), 0, 0 },
                    { new Guid("a3f92296-3241-497e-9153-59c0b3b8ab89"), 1, 5 },
                    { new Guid("b1a43278-516b-491e-bc08-900c3bf5dbdb"), 0, 3 },
                    { new Guid("ce741393-e66a-4d04-9b9e-0aef620ec39f"), 0, 4 },
                    { new Guid("e907e222-c36c-4f56-acc6-96fa237b6362"), 1, 8 },
                    { new Guid("e9f70412-4243-4aa6-adf4-c7a96eb566d4"), 1, 9 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("3266708a-0bd3-4fb2-84fd-b4e826ca2516"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("4b07110d-aed3-4ecd-9158-80376b25c351"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("5234a0c6-d8a8-4f00-abf7-bfe003c9c079"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("5a4ccdaa-1553-420c-9810-30fe3543fb37"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("66a197ae-bba9-4693-a019-317dfc7ae08a"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("7188600d-7729-443b-9293-576c1809f4c0"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("9696a0c1-9856-49d3-bf82-067e66a5e0fc"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("a3f92296-3241-497e-9153-59c0b3b8ab89"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("b1a43278-516b-491e-bc08-900c3bf5dbdb"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("ce741393-e66a-4d04-9b9e-0aef620ec39f"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("e907e222-c36c-4f56-acc6-96fa237b6362"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("e9f70412-4243-4aa6-adf4-c7a96eb566d4"));

            migrationBuilder.InsertData(
                table: "FacilityRatings",
                columns: new[] { "Id", "FacilityTypeId", "RatingId" },
                values: new object[,]
                {
                    { new Guid("2082d4e0-3c81-4f09-a1ee-71c9fbe68695"), 0, 0 },
                    { new Guid("220c785d-85dc-47f9-bd71-84527244ea26"), 1, 8 },
                    { new Guid("4db6fc84-4f56-41be-b451-a90175cbb330"), 1, 9 },
                    { new Guid("90260827-f4a3-4e51-b663-e1e3a04319ee"), 0, 3 },
                    { new Guid("98deef3f-1f0c-42b5-8f6e-691e42599055"), 1, 6 },
                    { new Guid("a3cec749-e989-4dfc-a951-01ae5968643e"), 1, 5 },
                    { new Guid("bb0ce269-6f65-4ad9-a78c-e2d875a3e615"), 0, 4 },
                    { new Guid("c3f8197d-dc54-42d3-a7ac-03b7a54f149f"), 0, 2 },
                    { new Guid("c9140b0f-aa3d-4356-acc6-1d7be690ab69"), 1, 7 },
                    { new Guid("e54995b3-4356-4d8e-9a85-3ea84708b622"), 0, 0 },
                    { new Guid("eaf29dae-91b6-4937-bf98-73dea569608d"), 0, 1 },
                    { new Guid("fb360e5e-00c7-4590-b382-5c2488436f27"), 2, 11 }
                });
        }
    }
}
