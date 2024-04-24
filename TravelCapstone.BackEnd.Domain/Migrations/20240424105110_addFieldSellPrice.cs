using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class addFieldSellPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellPriceHistorys_FacilityServices_FacilityServiceId",
                table: "SellPriceHistorys");

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

            migrationBuilder.AlterColumn<Guid>(
                name: "FacilityServiceId",
                table: "SellPriceHistorys",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "MenuId",
                table: "SellPriceHistorys",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TransportServiceDetailId",
                table: "SellPriceHistorys",
                type: "uniqueidentifier",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_SellPriceHistorys_MenuId",
                table: "SellPriceHistorys",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_SellPriceHistorys_TransportServiceDetailId",
                table: "SellPriceHistorys",
                column: "TransportServiceDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_SellPriceHistorys_FacilityServices_FacilityServiceId",
                table: "SellPriceHistorys",
                column: "FacilityServiceId",
                principalTable: "FacilityServices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SellPriceHistorys_Menus_MenuId",
                table: "SellPriceHistorys",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SellPriceHistorys_TransportServiceDetails_TransportServiceDetailId",
                table: "SellPriceHistorys",
                column: "TransportServiceDetailId",
                principalTable: "TransportServiceDetails",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellPriceHistorys_FacilityServices_FacilityServiceId",
                table: "SellPriceHistorys");

            migrationBuilder.DropForeignKey(
                name: "FK_SellPriceHistorys_Menus_MenuId",
                table: "SellPriceHistorys");

            migrationBuilder.DropForeignKey(
                name: "FK_SellPriceHistorys_TransportServiceDetails_TransportServiceDetailId",
                table: "SellPriceHistorys");

            migrationBuilder.DropIndex(
                name: "IX_SellPriceHistorys_MenuId",
                table: "SellPriceHistorys");

            migrationBuilder.DropIndex(
                name: "IX_SellPriceHistorys_TransportServiceDetailId",
                table: "SellPriceHistorys");

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

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "SellPriceHistorys");

            migrationBuilder.DropColumn(
                name: "TransportServiceDetailId",
                table: "SellPriceHistorys");

            migrationBuilder.AlterColumn<Guid>(
                name: "FacilityServiceId",
                table: "SellPriceHistorys",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_SellPriceHistorys_FacilityServices_FacilityServiceId",
                table: "SellPriceHistorys",
                column: "FacilityServiceId",
                principalTable: "FacilityServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
