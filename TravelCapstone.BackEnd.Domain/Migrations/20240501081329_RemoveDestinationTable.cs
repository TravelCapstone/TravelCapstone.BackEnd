using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class RemoveDestinationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Destinations_EndPointId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Destinations_StartPointId",
                table: "Routes");

            migrationBuilder.DropTable(
                name: "Destinations");

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

            migrationBuilder.AddColumn<int>(
                name: "SeatCapacity",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                    { new Guid("d39d6346-6831-4155-b27a-a2a779271ba2"), 0, 10 },
                    { new Guid("ef3fc5d9-7c7a-44c3-8fc1-8da12ca7e93b"), 0, 3 },
                    { new Guid("f2603607-d947-4aca-888e-9fd3bc3c0339"), 0, 4 },
                    { new Guid("f48d19ee-36ae-49bd-88ed-a3d326b1b1d8"), 1, 5 },
                    { new Guid("fa63bf87-51a3-46b7-a6df-aa29fb9b9057"), 1, 8 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Facilities_EndPointId",
                table: "Routes",
                column: "EndPointId",
                principalTable: "Facilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Facilities_StartPointId",
                table: "Routes",
                column: "StartPointId",
                principalTable: "Facilities",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Facilities_EndPointId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Facilities_StartPointId",
                table: "Routes");

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
                name: "SeatCapacity",
                table: "Vehicles");

            migrationBuilder.CreateTable(
                name: "Destinations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommunceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Destinations_Communes_CommunceId",
                        column: x => x.CommunceId,
                        principalTable: "Communes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_CommunceId",
                table: "Destinations",
                column: "CommunceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Destinations_EndPointId",
                table: "Routes",
                column: "EndPointId",
                principalTable: "Destinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Destinations_StartPointId",
                table: "Routes",
                column: "StartPointId",
                principalTable: "Destinations",
                principalColumn: "Id");
        }
    }
}
