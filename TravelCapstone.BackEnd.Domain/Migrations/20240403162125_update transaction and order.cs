using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class updatetransactionandorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.AddColumn<Guid>(
                name: "TravelCompanionId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TourId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TravelCompanionId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TravelCompanionId",
                table: "Transactions",
                column: "TravelCompanionId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TourId",
                table: "Orders",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TravelCompanionId",
                table: "Orders",
                column: "TravelCompanionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Tours_TourId",
                table: "Orders",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_TravelCompanions_TravelCompanionId",
                table: "Orders",
                column: "TravelCompanionId",
                principalTable: "TravelCompanions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_TravelCompanions_TravelCompanionId",
                table: "Transactions",
                column: "TravelCompanionId",
                principalTable: "TravelCompanions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Tours_TourId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_TravelCompanions_TravelCompanionId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_TravelCompanions_TravelCompanionId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_TravelCompanionId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Orders_TourId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_TravelCompanionId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TravelCompanionId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TourId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TravelCompanionId",
                table: "Orders");

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TravelCompanionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_TravelCompanions_TravelCompanionId",
                        column: x => x.TravelCompanionId,
                        principalTable: "TravelCompanions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_TravelCompanionId",
                table: "OrderDetails",
                column: "TravelCompanionId");
        }
    }
}