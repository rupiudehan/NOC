using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class PartiallyApprovedOn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PartiallyApprovedBy",
                table: "GrantDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartiallyApprovedByRole",
                table: "GrantDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PartiallyApprovedOn",
                table: "GrantDetails",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "PartiallyApprovedBy",
                table: "GrantDetails");

            migrationBuilder.DropColumn(
                name: "PartiallyApprovedByRole",
                table: "GrantDetails");

            migrationBuilder.DropColumn(
                name: "PartiallyApprovedOn",
                table: "GrantDetails");
        }
    }
}
