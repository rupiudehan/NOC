using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class shortfall : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsShortFall",
                table: "GrantDetails",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsShortFallCompleted",
                table: "GrantDetails",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ShortFallCompletedOn",
                table: "GrantDetails",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ShortFallLevel",
                table: "GrantDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ShortFallReportedById",
                table: "GrantDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortFallReportedByName",
                table: "GrantDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortFallReportedByRole",
                table: "GrantDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ShortFallReportedOn",
                table: "GrantDetails",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "LGD_ID",
                value: 27);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 2,
                column: "LGD_ID",
                value: 605);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 3,
                column: "LGD_ID",
                value: 28);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 4,
                column: "LGD_ID",
                value: 29);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 5,
                column: "LGD_ID",
                value: 30);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 6,
                column: "LGD_ID",
                value: 651);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 7,
                column: "LGD_ID",
                value: 31);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 8,
                column: "LGD_ID",
                value: 32);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 9,
                column: "LGD_ID",
                value: 33);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 10,
                column: "LGD_ID",
                value: 34);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 11,
                column: "LGD_ID",
                value: 35);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 12,
                column: "LGD_ID",
                value: 36);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 13,
                column: "LGD_ID",
                value: 737);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 14,
                column: "LGD_ID",
                value: 37);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 15,
                column: "LGD_ID",
                value: 38);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 16,
                column: "LGD_ID",
                value: 662);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 17,
                column: "LGD_ID",
                value: 41);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 18,
                column: "LGD_ID",
                value: 42);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 19,
                column: "LGD_ID",
                value: 608);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 20,
                column: "LGD_ID",
                value: 43);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 21,
                column: "LGD_ID",
                value: 40);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 22,
                column: "LGD_ID",
                value: 39);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 23,
                column: "LGD_ID",
                value: 609);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "IsShortFall",
                table: "GrantDetails");

            migrationBuilder.DropColumn(
                name: "IsShortFallCompleted",
                table: "GrantDetails");

            migrationBuilder.DropColumn(
                name: "ShortFallCompletedOn",
                table: "GrantDetails");

            migrationBuilder.DropColumn(
                name: "ShortFallLevel",
                table: "GrantDetails");

            migrationBuilder.DropColumn(
                name: "ShortFallReportedById",
                table: "GrantDetails");

            migrationBuilder.DropColumn(
                name: "ShortFallReportedByName",
                table: "GrantDetails");

            migrationBuilder.DropColumn(
                name: "ShortFallReportedByRole",
                table: "GrantDetails");

            migrationBuilder.DropColumn(
                name: "ShortFallReportedOn",
                table: "GrantDetails");

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 2,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 3,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 4,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 5,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 6,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 7,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 8,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 9,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 10,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 11,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 12,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 13,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 14,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 15,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 16,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 17,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 18,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 19,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 20,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 21,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 22,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 23,
                column: "LGD_ID",
                value: 0);
        }
    }
}
