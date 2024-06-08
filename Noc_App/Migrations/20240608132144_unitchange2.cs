using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class unitchange2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DivideBy", "UnitValue" },
                values: new object[] { 32.270000000000003, 1.0 });

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DivideBy", "UnitValue" },
                values: new object[] { 1.673, 1.0 });

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DivideBy", "UnitValue" },
                values: new object[] { 32.310000000000002, 1.0 });

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DivideBy", "UnitValue" },
                values: new object[] { 32.270000000000003, 1.0 });

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DivideBy", "UnitValue" },
                values: new object[] { 1.673, 1.0 });

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DivideBy", "UnitValue" },
                values: new object[] { 32.310000000000002, 1.0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DivideBy", "UnitValue" },
                values: new object[] { 1.0, 32.270000000000003 });

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DivideBy", "UnitValue" },
                values: new object[] { 1.0, 1.673 });

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DivideBy", "UnitValue" },
                values: new object[] { 1.0, 32.310000000000002 });

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DivideBy", "UnitValue" },
                values: new object[] { 1.0, 32.270000000000003 });

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DivideBy", "UnitValue" },
                values: new object[] { 1.0, 1.673 });

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DivideBy", "UnitValue" },
                values: new object[] { 1.0, 32.310000000000002 });
        }
    }
}
