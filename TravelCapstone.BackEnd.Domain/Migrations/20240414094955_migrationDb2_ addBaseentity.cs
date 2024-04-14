using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCapstone.BackEnd.Domain.Migrations
{
    public partial class migrationDb2_addBaseentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreateBy",
                table: "PrivateTourRequests",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "PrivateTourRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdateBy",
                table: "PrivateTourRequests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "PrivateTourRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateBy",
                table: "PrivateJoinTourRequests",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "PrivateJoinTourRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdateBy",
                table: "PrivateJoinTourRequests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "PrivateJoinTourRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_PrivateTourRequests_CreateBy",
                table: "PrivateTourRequests",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateTourRequests_UpdateBy",
                table: "PrivateTourRequests",
                column: "UpdateBy");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateJoinTourRequests_CreateBy",
                table: "PrivateJoinTourRequests",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateJoinTourRequests_UpdateBy",
                table: "PrivateJoinTourRequests",
                column: "UpdateBy");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateJoinTourRequests_AspNetUsers_CreateBy",
                table: "PrivateJoinTourRequests",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateJoinTourRequests_AspNetUsers_UpdateBy",
                table: "PrivateJoinTourRequests",
                column: "UpdateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateTourRequests_AspNetUsers_CreateBy",
                table: "PrivateTourRequests",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateTourRequests_AspNetUsers_UpdateBy",
                table: "PrivateTourRequests",
                column: "UpdateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrivateJoinTourRequests_AspNetUsers_CreateBy",
                table: "PrivateJoinTourRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateJoinTourRequests_AspNetUsers_UpdateBy",
                table: "PrivateJoinTourRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateTourRequests_AspNetUsers_CreateBy",
                table: "PrivateTourRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateTourRequests_AspNetUsers_UpdateBy",
                table: "PrivateTourRequests");

            migrationBuilder.DropIndex(
                name: "IX_PrivateTourRequests_CreateBy",
                table: "PrivateTourRequests");

            migrationBuilder.DropIndex(
                name: "IX_PrivateTourRequests_UpdateBy",
                table: "PrivateTourRequests");

            migrationBuilder.DropIndex(
                name: "IX_PrivateJoinTourRequests_CreateBy",
                table: "PrivateJoinTourRequests");

            migrationBuilder.DropIndex(
                name: "IX_PrivateJoinTourRequests_UpdateBy",
                table: "PrivateJoinTourRequests");

            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "PrivateTourRequests");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "PrivateTourRequests");

            migrationBuilder.DropColumn(
                name: "UpdateBy",
                table: "PrivateTourRequests");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "PrivateTourRequests");

            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "PrivateJoinTourRequests");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "PrivateJoinTourRequests");

            migrationBuilder.DropColumn(
                name: "UpdateBy",
                table: "PrivateJoinTourRequests");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "PrivateJoinTourRequests");
        }
    }
}
