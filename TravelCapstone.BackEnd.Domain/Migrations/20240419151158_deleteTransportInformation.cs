using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class deleteTransportInformation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransportInformations");

            migrationBuilder.CreateTable(
                name: "VehicleQuotationDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleType = table.Column<int>(type: "int", nullable: false),
                    StartPointId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EndPointId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MinPrice = table.Column<double>(type: "float", nullable: false),
                    MaxPrice = table.Column<double>(type: "float", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OptionQuotationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleQuotationDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleQuotationDetails_OptionQuotations_OptionQuotationId",
                        column: x => x.OptionQuotationId,
                        principalTable: "OptionQuotations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleQuotationDetails_Provinces_EndPointId",
                        column: x => x.EndPointId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleQuotationDetails_Provinces_StartPointId",
                        column: x => x.StartPointId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_VehicleQuotationDetails_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleQuotationDetails_EndPointId",
                table: "VehicleQuotationDetails",
                column: "EndPointId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleQuotationDetails_OptionQuotationId",
                table: "VehicleQuotationDetails",
                column: "OptionQuotationId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleQuotationDetails_ServiceId",
                table: "VehicleQuotationDetails",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleQuotationDetails_StartPointId",
                table: "VehicleQuotationDetails",
                column: "StartPointId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VehicleQuotationDetails");

            migrationBuilder.CreateTable(
                name: "TransportInformations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EndPointId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuotationDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartPointId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                        onDelete: ReferentialAction.Cascade);
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
    }
}
