using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class AddFieldFacilityServiceToTransportDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<Guid>(
                name: "FacilityServiceId",
                table: "TransportServiceDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "CreateBy",
                table: "Menus",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Menus",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdateBy",
                table: "Menus",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Menus",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "FacilityServices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "FacilityRatings",
                columns: new[] { "Id", "FacilityTypeId", "RatingId" },
                values: new object[,]
                {
                    { new Guid("11c6720b-6040-4b4d-bece-7c7a57960f59"), 1, 8 },
                    { new Guid("34b6e168-13b0-4f4e-bf0b-466c3b0ad024"), 1, 7 },
                    { new Guid("4f6da9e1-e1ae-40cc-9bbf-0e4f3f1f0612"), 0, 0 },
                    { new Guid("5a883a5f-d485-4de1-8afa-a3fe5c9888a2"), 0, 2 },
                    { new Guid("695e80ff-4ffe-4edd-88c0-64daf9825648"), 2, 11 },
                    { new Guid("78515336-6fb8-458c-9cc2-a5bbd897f671"), 0, 4 },
                    { new Guid("8a93a06c-83c8-49fe-a772-9c95787ecac9"), 1, 6 },
                    { new Guid("8ea82025-f43c-4eba-b60f-5cc67436e028"), 0, 0 },
                    { new Guid("9140c68a-e11b-4dbe-83c1-41f5bc01e30d"), 0, 3 },
                    { new Guid("a10e4edc-54b6-44ac-8fd2-5f7f450f3ac7"), 0, 1 },
                    { new Guid("f4ae7a28-9277-440e-b07a-4ab70c075669"), 1, 9 },
                    { new Guid("f9044258-c1ef-45d9-a427-0adef716150b"), 1, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransportServiceDetails_FacilityServiceId",
                table: "TransportServiceDetails",
                column: "FacilityServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_CreateBy",
                table: "Menus",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_UpdateBy",
                table: "Menus",
                column: "UpdateBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_AspNetUsers_CreateBy",
                table: "Menus",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_AspNetUsers_UpdateBy",
                table: "Menus",
                column: "UpdateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransportServiceDetails_FacilityServices_FacilityServiceId",
                table: "TransportServiceDetails",
                column: "FacilityServiceId",
                principalTable: "FacilityServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menus_AspNetUsers_CreateBy",
                table: "Menus");

            migrationBuilder.DropForeignKey(
                name: "FK_Menus_AspNetUsers_UpdateBy",
                table: "Menus");

            migrationBuilder.DropForeignKey(
                name: "FK_TransportServiceDetails_FacilityServices_FacilityServiceId",
                table: "TransportServiceDetails");

            migrationBuilder.DropIndex(
                name: "IX_TransportServiceDetails_FacilityServiceId",
                table: "TransportServiceDetails");

            migrationBuilder.DropIndex(
                name: "IX_Menus_CreateBy",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_UpdateBy",
                table: "Menus");

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("11c6720b-6040-4b4d-bece-7c7a57960f59"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("34b6e168-13b0-4f4e-bf0b-466c3b0ad024"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("4f6da9e1-e1ae-40cc-9bbf-0e4f3f1f0612"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("5a883a5f-d485-4de1-8afa-a3fe5c9888a2"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("695e80ff-4ffe-4edd-88c0-64daf9825648"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("78515336-6fb8-458c-9cc2-a5bbd897f671"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("8a93a06c-83c8-49fe-a772-9c95787ecac9"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("8ea82025-f43c-4eba-b60f-5cc67436e028"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("9140c68a-e11b-4dbe-83c1-41f5bc01e30d"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("a10e4edc-54b6-44ac-8fd2-5f7f450f3ac7"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("f4ae7a28-9277-440e-b07a-4ab70c075669"));

            migrationBuilder.DeleteData(
                table: "FacilityRatings",
                keyColumn: "Id",
                keyValue: new Guid("f9044258-c1ef-45d9-a427-0adef716150b"));

            migrationBuilder.DropColumn(
                name: "FacilityServiceId",
                table: "TransportServiceDetails");

            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "UpdateBy",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "FacilityServices");

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
    }
}
