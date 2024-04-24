using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class AddTransportDetailFieldToSellprice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("0ad42b1c-4aec-4d4d-8f87-b34a98e8a2b4"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("1a4b1d62-be99-40fd-8409-92dd85373df7"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("1ac094c0-bfeb-4fc0-8a58-9086a39d0694"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("208691cd-ce4e-4de9-a3ed-ffd2781cb38e"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("466bf28c-17ba-4fcc-8022-d3b0c630d717"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("61af1806-dec0-4407-86ed-15990c6ad878"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("72190bff-0edf-420f-909e-a02761d0518e"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("7a6ec4d6-2e18-4091-ae3e-9294bc23d4b2"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("821ce91f-fb44-474c-95b1-d4b7e604af1a"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("b0ce9d89-7498-4515-b396-f8df73878933"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("c9184b75-00e9-4732-8436-c5b3b083506e"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("e94f7c4b-3e00-4ea9-a909-038a76021390"));

            migrationBuilder.InsertData(
                table: "FacilityRatings",
                columns: new[] { "Id", "FacilityTypeId", "RatingId" },
                values: new object[,]
                {
                    { new Guid("0118fa06-3a1d-4bf3-ac3f-7570f67489c3"), 1, 9 },
                    { new Guid("025fcf90-85d0-4d31-9245-e5f777ae3fc9"), 1, 5 },
                    { new Guid("11f15dad-a1f9-44ea-bf6e-296d418aca8d"), 1, 6 },
                    { new Guid("3191d7ee-b056-4c34-aa73-4a27d2df2858"), 1, 7 },
                    { new Guid("3b502802-09e8-4b2a-8b25-8a37c456610e"), 1, 8 },
                    { new Guid("5128f689-58ac-4bbd-aa8a-a4a7cf6d066b"), 0, 0 },
                    { new Guid("5c8e041d-2988-4620-912d-6249dc6e8a15"), 0, 3 },
                    { new Guid("5e84043c-1126-428d-b6fd-f4c350b57035"), 0, 1 },
                    { new Guid("6a50ea39-802c-43d2-94af-c0d2fa6a3c95"), 2, 11 },
                    { new Guid("79cdf912-f710-45fb-9130-04badeaf3103"), 0, 2 },
                    { new Guid("ba5ae6d6-27e2-4ce6-8f4f-2a87f5f454c3"), 0, 4 },
                    { new Guid("eb10328c-ead0-446f-90fa-970f4c91fe11"), 0, 0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("0118fa06-3a1d-4bf3-ac3f-7570f67489c3"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("025fcf90-85d0-4d31-9245-e5f777ae3fc9"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("11f15dad-a1f9-44ea-bf6e-296d418aca8d"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("3191d7ee-b056-4c34-aa73-4a27d2df2858"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("3b502802-09e8-4b2a-8b25-8a37c456610e"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("5128f689-58ac-4bbd-aa8a-a4a7cf6d066b"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("5c8e041d-2988-4620-912d-6249dc6e8a15"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("5e84043c-1126-428d-b6fd-f4c350b57035"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("6a50ea39-802c-43d2-94af-c0d2fa6a3c95"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("79cdf912-f710-45fb-9130-04badeaf3103"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("ba5ae6d6-27e2-4ce6-8f4f-2a87f5f454c3"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("eb10328c-ead0-446f-90fa-970f4c91fe11"));

            migrationBuilder.InsertData(
                table: "FacilityRatings",
                columns: new[] { "Id", "FacilityTypeId", "RatingId" },
                values: new object[,]
                {
                    { new Guid("0ad42b1c-4aec-4d4d-8f87-b34a98e8a2b4"), 1, 8 },
                    { new Guid("1a4b1d62-be99-40fd-8409-92dd85373df7"), 2, 11 },
                    { new Guid("1ac094c0-bfeb-4fc0-8a58-9086a39d0694"), 1, 6 },
                    { new Guid("208691cd-ce4e-4de9-a3ed-ffd2781cb38e"), 0, 1 },
                    { new Guid("466bf28c-17ba-4fcc-8022-d3b0c630d717"), 1, 9 },
                    { new Guid("61af1806-dec0-4407-86ed-15990c6ad878"), 1, 7 },
                    { new Guid("72190bff-0edf-420f-909e-a02761d0518e"), 0, 4 },
                    { new Guid("7a6ec4d6-2e18-4091-ae3e-9294bc23d4b2"), 0, 0 },
                    { new Guid("821ce91f-fb44-474c-95b1-d4b7e604af1a"), 0, 3 },
                    { new Guid("b0ce9d89-7498-4515-b396-f8df73878933"), 0, 2 },
                    { new Guid("c9184b75-00e9-4732-8436-c5b3b083506e"), 0, 0 },
                    { new Guid("e94f7c4b-3e00-4ea9-a909-038a76021390"), 1, 5 }
                });
        }
    }
}
