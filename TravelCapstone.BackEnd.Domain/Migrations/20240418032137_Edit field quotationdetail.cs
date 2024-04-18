using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class Editfieldquotationdetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlightInformations");

            migrationBuilder.CreateTable(
                name: "TransportInformations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartPointId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EndPointId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuotationDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportInformations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportInformations_Provinces_EndPointId",
                        column: x => x.EndPointId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransportInformations_Provinces_StartPointId",
                        column: x => x.StartPointId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TransportInformations_QuotationDetails_QuotationDetailId",
                        column: x => x.QuotationDetailId,
                        principalTable: "QuotationDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransportInformations_EndPointId",
                table: "TransportInformations",
                column: "EndPointId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportInformations_QuotationDetailId",
                table: "TransportInformations",
                column: "QuotationDetailId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransportInformations_StartPointId",
                table: "TransportInformations",
                column: "StartPointId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransportInformations");

            migrationBuilder.CreateTable(
                name: "FlightInformations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuotationDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EndPoint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartPoint = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightInformations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlightInformations_QuotationDetails_QuotationDetailId",
                        column: x => x.QuotationDetailId,
                        principalTable: "QuotationDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FlightInformations_QuotationDetailId",
                table: "FlightInformations",
                column: "QuotationDetailId",
                unique: true);
        }
    }
}
