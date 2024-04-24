using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class AddMenuFieldToSellprice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("0f9d86b6-15b4-4515-9c73-e76ba1b1b9db"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("164c7400-0352-4fa4-8364-1eed2581b119"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("1f96e29a-f30b-4a7c-bef5-03655578a5c0"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("35f4a977-6d58-4332-9ebd-59fabe683e3c"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("41f6e693-d06c-43bb-934c-8d3dc014dbba"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("6b1d4940-bb49-44cf-9a1e-59b69c035355"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("6f6c405b-3f1e-48c0-b0c0-6b532c232023"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("aeece871-67b1-41e9-b909-d4545457b53a"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("b214adff-95e2-430f-9091-810bc3bce5ca"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("b7f55d43-0987-4198-ab38-192dd1979685"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("bf21c56a-8e84-4280-8e60-6e507c9830ad"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("dba3eb35-4f27-45a5-a9e5-d81d68ca8c69"));

            migrationBuilder.AddColumn<Guid>(
                name: "MenuId",
                table: "SellPriceHistorys",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "FacilityRatings",
                columns: new[] { "Id", "FacilityTypeId", "RatingId" },
                values: new object[,]
                {
                    { new Guid("1ba853db-84e4-4e9c-b750-dbe432d1a2b0"), 0, 2 },
                    { new Guid("1e955290-e063-439f-8e17-d2edcb2ad69b"), 0, 1 },
                    { new Guid("23c8e757-ac2d-4232-a569-d5461deb9b05"), 1, 6 },
                    { new Guid("34115f50-25d5-4750-8500-5cb917f27da5"), 0, 0 },
                    { new Guid("4b3961ff-aa19-41fd-9a3e-1a6b37f99d09"), 1, 9 },
                    { new Guid("994cb1a9-6d4e-4d34-b904-91b229a12d5c"), 2, 11 },
                    { new Guid("a59bbb50-4fcc-473f-bbf4-b311f06b2ed8"), 1, 7 },
                    { new Guid("d39d6346-6831-4155-b27a-a2a779271ba2"), 0, 0 },
                    { new Guid("ef3fc5d9-7c7a-44c3-8fc1-8da12ca7e93b"), 0, 3 },
                    { new Guid("f2603607-d947-4aca-888e-9fd3bc3c0339"), 0, 4 },
                    { new Guid("f48d19ee-36ae-49bd-88ed-a3d326b1b1d8"), 1, 5 },
                    { new Guid("fa63bf87-51a3-46b7-a6df-aa29fb9b9057"), 1, 8 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SellPriceHistorys_MenuId",
                table: "SellPriceHistorys",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_SellPriceHistorys_Menus_MenuId",
                table: "SellPriceHistorys",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellPriceHistorys_Menus_MenuId",
                table: "SellPriceHistorys");

            migrationBuilder.DropIndex(
                name: "IX_SellPriceHistorys_MenuId",
                table: "SellPriceHistorys");

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("1ba853db-84e4-4e9c-b750-dbe432d1a2b0"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("1e955290-e063-439f-8e17-d2edcb2ad69b"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("23c8e757-ac2d-4232-a569-d5461deb9b05"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("34115f50-25d5-4750-8500-5cb917f27da5"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("4b3961ff-aa19-41fd-9a3e-1a6b37f99d09"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("994cb1a9-6d4e-4d34-b904-91b229a12d5c"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("a59bbb50-4fcc-473f-bbf4-b311f06b2ed8"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("d39d6346-6831-4155-b27a-a2a779271ba2"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("ef3fc5d9-7c7a-44c3-8fc1-8da12ca7e93b"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("f2603607-d947-4aca-888e-9fd3bc3c0339"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("f48d19ee-36ae-49bd-88ed-a3d326b1b1d8"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("fa63bf87-51a3-46b7-a6df-aa29fb9b9057"));

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "SellPriceHistorys");

            migrationBuilder.InsertData(
                table: "FacilityRatings",
                columns: new[] { "Id", "FacilityTypeId", "RatingId" },
                values: new object[,]
                {
                    { new Guid("0f9d86b6-15b4-4515-9c73-e76ba1b1b9db"), 0, 0 },
                    { new Guid("164c7400-0352-4fa4-8364-1eed2581b119"), 1, 7 },
                    { new Guid("1f96e29a-f30b-4a7c-bef5-03655578a5c0"), 1, 6 },
                    { new Guid("35f4a977-6d58-4332-9ebd-59fabe683e3c"), 0, 4 },
                    { new Guid("41f6e693-d06c-43bb-934c-8d3dc014dbba"), 0, 0 },
                    { new Guid("6b1d4940-bb49-44cf-9a1e-59b69c035355"), 0, 3 },
                    { new Guid("6f6c405b-3f1e-48c0-b0c0-6b532c232023"), 2, 11 },
                    { new Guid("aeece871-67b1-41e9-b909-d4545457b53a"), 0, 1 },
                    { new Guid("b214adff-95e2-430f-9091-810bc3bce5ca"), 1, 5 },
                    { new Guid("b7f55d43-0987-4198-ab38-192dd1979685"), 1, 9 },
                    { new Guid("bf21c56a-8e84-4280-8e60-6e507c9830ad"), 1, 8 },
                    { new Guid("dba3eb35-4f27-45a5-a9e5-d81d68ca8c69"), 0, 2 }
                });
        }
    }
}
