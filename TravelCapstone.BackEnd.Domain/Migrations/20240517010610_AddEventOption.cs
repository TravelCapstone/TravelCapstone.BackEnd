using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class AddEventOption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_OptionQuotations_OptionId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_OptionId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "OptionId",
                table: "Events");

            migrationBuilder.CreateTable(
                name: "OptionEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OptionEvents_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_OptionEvents_OptionQuotations_OptionId",
                        column: x => x.OptionId,
                        principalTable: "OptionQuotations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OptionEvents_EventId",
                table: "OptionEvents",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionEvents_OptionId",
                table: "OptionEvents",
                column: "OptionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OptionEvents");

            migrationBuilder.AddColumn<Guid>(
                name: "OptionId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_OptionId",
                table: "Events",
                column: "OptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_OptionQuotations_OptionId",
                table: "Events",
                column: "OptionId",
                principalTable: "OptionQuotations",
                principalColumn: "Id");
        }
    }
}
