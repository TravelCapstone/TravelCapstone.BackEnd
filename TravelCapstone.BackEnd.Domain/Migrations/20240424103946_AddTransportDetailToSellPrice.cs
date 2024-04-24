using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class AddTransportDetailToSellPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { new Guid("21b4f49e-ef45-40cc-8c40-6338c4ecc9c8"), 1, 9 },
                    { new Guid("23ae0642-c77d-4541-88be-a57c8a705291"), 1, 7 },
                    { new Guid("2a3266af-f914-406f-81e6-b667057749ad"), 1, 6 },
                    { new Guid("3b437453-cff5-4647-aa1e-aa52de547456"), 2, 11 },
                    { new Guid("648a0765-3ee5-4ee3-998b-cdfe8308405a"), 0, 0 },
                    { new Guid("86f7af62-5bed-43b1-a23a-bc136577896a"), 0, 3 },
                    { new Guid("8e2c408b-699e-4a5f-afa2-d1d8863e54d6"), 0, 4 },
                    { new Guid("bdab04c3-e35a-405c-8784-a947e8492e06"), 1, 8 },
                    { new Guid("c2aa1fd2-7173-4c53-aa0d-fa77565c2ad8"), 0, 2 },
                    { new Guid("d3fb7bbd-be9f-45b4-b539-06a2c419a52b"), 0, 0 },
                    { new Guid("d5deb4aa-f634-40f0-b74e-c42ad2b67f37"), 0, 1 },
                    { new Guid("e1a7b0b8-f9eb-4db0-84ae-1d2e32d92c5c"), 1, 5 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("21b4f49e-ef45-40cc-8c40-6338c4ecc9c8"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("23ae0642-c77d-4541-88be-a57c8a705291"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("2a3266af-f914-406f-81e6-b667057749ad"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("3b437453-cff5-4647-aa1e-aa52de547456"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("648a0765-3ee5-4ee3-998b-cdfe8308405a"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("86f7af62-5bed-43b1-a23a-bc136577896a"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("8e2c408b-699e-4a5f-afa2-d1d8863e54d6"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("bdab04c3-e35a-405c-8784-a947e8492e06"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("c2aa1fd2-7173-4c53-aa0d-fa77565c2ad8"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("d3fb7bbd-be9f-45b4-b539-06a2c419a52b"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("d5deb4aa-f634-40f0-b74e-c42ad2b67f37"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("e1a7b0b8-f9eb-4db0-84ae-1d2e32d92c5c"));

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
    }
}
