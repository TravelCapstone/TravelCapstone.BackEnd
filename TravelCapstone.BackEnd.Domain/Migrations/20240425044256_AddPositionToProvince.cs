using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class AddPositionToProvince : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<double>(
                name: "lat",
                table: "Provinces",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "lng",
                table: "Provinces",
                type: "float",
                nullable: true);

            migrationBuilder.InsertData(
                table: "FacilityRatings",
                columns: new[] { "Id", "FacilityTypeId", "RatingId" },
                values: new object[,]
                {
                    { new Guid("03988dad-131f-40e3-9c3d-53441de26f10"), 1, 5 },
                    { new Guid("04e73eef-5357-4a96-b381-3d3acc7a0162"), 0, 1 },
                    { new Guid("07a548d9-2cc0-43db-9955-9baa4416bbe1"), 0, 0 },
                    { new Guid("16d8dc55-6fba-4f14-b772-4e59e6774814"), 2, 11 },
                    { new Guid("22167a65-1bb7-459f-b139-0199da0d90ab"), 0, 0 },
                    { new Guid("3fec69a2-b513-4160-81e1-a8b6e704dc99"), 1, 7 },
                    { new Guid("4577e53c-0800-4394-8a34-4b3e79583f7b"), 0, 4 },
                    { new Guid("482745d0-1ff6-4d39-80e5-96c3824acf58"), 1, 6 },
                    { new Guid("4c981c59-f529-4858-8c8d-d665eeb5c785"), 1, 9 },
                    { new Guid("a1c17d13-a24a-487a-86de-2ffe54bc1b9f"), 1, 8 },
                    { new Guid("c2e0b315-7ae3-4fc0-a6fa-7c3464d02294"), 0, 2 },
                    { new Guid("e3946b7f-4117-4a88-bf44-3b0373336008"), 0, 3 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("03988dad-131f-40e3-9c3d-53441de26f10"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("04e73eef-5357-4a96-b381-3d3acc7a0162"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("07a548d9-2cc0-43db-9955-9baa4416bbe1"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("16d8dc55-6fba-4f14-b772-4e59e6774814"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("22167a65-1bb7-459f-b139-0199da0d90ab"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("3fec69a2-b513-4160-81e1-a8b6e704dc99"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("4577e53c-0800-4394-8a34-4b3e79583f7b"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("482745d0-1ff6-4d39-80e5-96c3824acf58"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("4c981c59-f529-4858-8c8d-d665eeb5c785"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("a1c17d13-a24a-487a-86de-2ffe54bc1b9f"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("c2e0b315-7ae3-4fc0-a6fa-7c3464d02294"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("e3946b7f-4117-4a88-bf44-3b0373336008"));

            migrationBuilder.DropColumn(
                name: "lat",
                table: "Provinces");

            migrationBuilder.DropColumn(
                name: "lng",
                table: "Provinces");

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
    }
}
