using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class updatedunitcodesinmaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "UnitCode", "UnitName" },
                values: new object[] { "M", "Biswa" });

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "UnitCode", "UnitName" },
                values: new object[] { "K", "Bigha" });

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "UnitCode", "UnitName" },
                values: new object[] { "S", "Biswansi" });

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "UnitCode", "UnitName" },
                values: new object[] { "M", "Biswa" });

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "UnitCode", "UnitName" },
                values: new object[] { "K", "Bigha" });

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "UnitCode", "UnitName" },
                values: new object[] { "S", "Biswansi" });

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 8,
                column: "UnitCode",
                value: "M");

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 9,
                column: "UnitCode",
                value: "S");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "UnitCode", "UnitName" },
                values: new object[] { "BSI", "Biswa-I" });

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "UnitCode", "UnitName" },
                values: new object[] { "BGI", "Bigha-I" });

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "UnitCode", "UnitName" },
                values: new object[] { "BWI", "Biswansi-I" });

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "UnitCode", "UnitName" },
                values: new object[] { "BSI", "Biswa-II" });

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "UnitCode", "UnitName" },
                values: new object[] { "BGI", "Bigha-II" });

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "UnitCode", "UnitName" },
                values: new object[] { "BWI", "Biswansi-II" });

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 8,
                column: "UnitCode",
                value: "K");

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 9,
                column: "UnitCode",
                value: "K");
        }
    }
}
