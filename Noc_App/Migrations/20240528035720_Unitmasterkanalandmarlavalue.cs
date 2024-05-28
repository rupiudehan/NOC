using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class Unitmasterkanalandmarlavalue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 7,
                column: "DivideBy",
                value: 8.0);

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 8,
                column: "DivideBy",
                value: 160.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 7,
                column: "DivideBy",
                value: 160.0);

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 8,
                column: "DivideBy",
                value: 8.0);
        }
    }
}
