using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class unitchange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 1,
                column: "UnitValue",
                value: 32.270000000000003);

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 2,
                column: "UnitValue",
                value: 1.673);

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 3,
                column: "UnitValue",
                value: 32.310000000000002);

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 4,
                column: "UnitValue",
                value: 32.270000000000003);

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 5,
                column: "UnitValue",
                value: 1.673);

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 6,
                column: "UnitValue",
                value: 32.310000000000002);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 1,
                column: "UnitValue",
                value: 0.012500000000000001);

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 2,
                column: "UnitValue",
                value: 0.25);

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 3,
                column: "UnitValue",
                value: 0.00062500000000000001);

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 4,
                column: "UnitValue",
                value: 0.012500000000000001);

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 5,
                column: "UnitValue",
                value: 0.25);

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 6,
                column: "UnitValue",
                value: 0.00062500000000000001);
        }
    }
}
