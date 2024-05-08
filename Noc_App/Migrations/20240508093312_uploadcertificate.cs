using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class uploadcertificate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "CertificateFilePath",
                table: "GrantDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UploadedBy",
                table: "GrantDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UploadedByRole",
                table: "GrantDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UploadedOn",
                table: "GrantDetails",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificateFilePath",
                table: "GrantDetails");

            migrationBuilder.DropColumn(
                name: "UploadedBy",
                table: "GrantDetails");

            migrationBuilder.DropColumn(
                name: "UploadedByRole",
                table: "GrantDetails");

            migrationBuilder.DropColumn(
                name: "UploadedOn",
                table: "GrantDetails");

        }
    }
}
