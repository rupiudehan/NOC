using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class fromlocationtolocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FromLocationId",
                table: "GrantFileTransferDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ToLocationId",
                table: "GrantFileTransferDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FromLocationId",
                table: "GrantApprovalDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ToLocationId",
                table: "GrantApprovalDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "FromLocationId",
                table: "GrantFileTransferDetails");

            migrationBuilder.DropColumn(
                name: "ToLocationId",
                table: "GrantFileTransferDetails");

            migrationBuilder.DropColumn(
                name: "FromLocationId",
                table: "GrantApprovalDetails");

            migrationBuilder.DropColumn(
                name: "ToLocationId",
                table: "GrantApprovalDetails");
        }
    }
}
