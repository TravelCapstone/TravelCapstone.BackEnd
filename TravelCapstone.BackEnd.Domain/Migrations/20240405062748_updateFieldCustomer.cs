using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class updateFieldCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_TravelCompanions_TravelCompanionId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateJoinTourRequests_TravelCompanions_TravelCompanionId",
                table: "PrivateJoinTourRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TourRegistrations_TravelCompanions_FollowerId",
                table: "TourRegistrations");

            migrationBuilder.DropForeignKey(
                name: "FK_TourRegistrations_TravelCompanions_PresenterId",
                table: "TourRegistrations");

            migrationBuilder.DropForeignKey(
                name: "FK_TourTravellers_TravelCompanions_TravelCompanionId",
                table: "TourTravellers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_TravelCompanions_TravelCompanionId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "TravelCompanions");

            migrationBuilder.RenameColumn(
                name: "TravelCompanionId",
                table: "TourTravellers",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_TourTravellers_TravelCompanionId",
                table: "TourTravellers",
                newName: "IX_TourTravellers_CustomerId");

            migrationBuilder.RenameColumn(
                name: "TravelCompanionId",
                table: "Orders",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_TravelCompanionId",
                table: "Orders",
                newName: "IX_Orders_CustomerId");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "PlanServiceCostDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<bool>(type: "bit", nullable: false),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAdult = table.Column<bool>(type: "bit", nullable: false),
                    Money = table.Column<double>(type: "float", nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsVerfiedPhoneNumber = table.Column<bool>(type: "bit", nullable: false),
                    IsVerifiedEmail = table.Column<bool>(type: "bit", nullable: false),
                    VerficationCodePhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerficationCodeEmail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_AspNetUsers_AccountId",
                        column: x => x.AccountId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_AccountId",
                table: "Customers",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateJoinTourRequests_Customers_TravelCompanionId",
                table: "PrivateJoinTourRequests",
                column: "TravelCompanionId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TourRegistrations_Customers_FollowerId",
                table: "TourRegistrations",
                column: "FollowerId",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TourRegistrations_Customers_PresenterId",
                table: "TourRegistrations",
                column: "PresenterId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TourTravellers_Customers_CustomerId",
                table: "TourTravellers",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Customers_TravelCompanionId",
                table: "Transactions",
                column: "TravelCompanionId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateJoinTourRequests_Customers_TravelCompanionId",
                table: "PrivateJoinTourRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TourRegistrations_Customers_FollowerId",
                table: "TourRegistrations");

            migrationBuilder.DropForeignKey(
                name: "FK_TourRegistrations_Customers_PresenterId",
                table: "TourRegistrations");

            migrationBuilder.DropForeignKey(
                name: "FK_TourTravellers_Customers_CustomerId",
                table: "TourTravellers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Customers_TravelCompanionId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "PlanServiceCostDetails");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "TourTravellers",
                newName: "TravelCompanionId");

            migrationBuilder.RenameIndex(
                name: "IX_TourTravellers_CustomerId",
                table: "TourTravellers",
                newName: "IX_TourTravellers_TravelCompanionId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Orders",
                newName: "TravelCompanionId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                newName: "IX_Orders_TravelCompanionId");

            migrationBuilder.CreateTable(
                name: "TravelCompanions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<bool>(type: "bit", nullable: false),
                    IsAdult = table.Column<bool>(type: "bit", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Money = table.Column<double>(type: "float", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelCompanions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TravelCompanions_AspNetUsers_AccountId",
                        column: x => x.AccountId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TravelCompanions_AccountId",
                table: "TravelCompanions",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_TravelCompanions_TravelCompanionId",
                table: "Orders",
                column: "TravelCompanionId",
                principalTable: "TravelCompanions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateJoinTourRequests_TravelCompanions_TravelCompanionId",
                table: "PrivateJoinTourRequests",
                column: "TravelCompanionId",
                principalTable: "TravelCompanions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TourRegistrations_TravelCompanions_FollowerId",
                table: "TourRegistrations",
                column: "FollowerId",
                principalTable: "TravelCompanions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TourRegistrations_TravelCompanions_PresenterId",
                table: "TourRegistrations",
                column: "PresenterId",
                principalTable: "TravelCompanions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TourTravellers_TravelCompanions_TravelCompanionId",
                table: "TourTravellers",
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
    }
}
