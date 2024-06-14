using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class addeddaysforadeandDD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "NoOfDays",
                table: "DaysCheckMaster",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.UpdateData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 1,
                column: "NoOfDays",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 2,
                column: "NoOfDays",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 3,
                column: "NoOfDays",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 4,
                column: "NoOfDays",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 5,
                column: "NoOfDays",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 6,
                column: "NoOfDays",
                value: 0.5);

            migrationBuilder.UpdateData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 7,
                column: "NoOfDays",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 8,
                column: "NoOfDays",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 9,
                column: "NoOfDays",
                value: 2.0);

            migrationBuilder.InsertData(
                table: "DaysCheckMaster",
                columns: new[] { "Id", "CheckFor", "Code", "IsRelatedToForward", "IsRelatedToIssue", "NoOfDays", "UserRoleID" },
                values: new object[,]
                {
                    { 10, "ADE/DWS", "ADE", 1, 0, 1.0, 90 },
                    { 11, "Director Drainage", "DD", 1, 0, 0.5, 35 }
                });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.AlterColumn<int>(
                name: "NoOfDays",
                table: "DaysCheckMaster",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.UpdateData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 1,
                column: "NoOfDays",
                value: 1);

            migrationBuilder.UpdateData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 2,
                column: "NoOfDays",
                value: 3);

            migrationBuilder.UpdateData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 3,
                column: "NoOfDays",
                value: 2);

            migrationBuilder.UpdateData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 4,
                column: "NoOfDays",
                value: 2);

            migrationBuilder.UpdateData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 5,
                column: "NoOfDays",
                value: 3);

            migrationBuilder.UpdateData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 6,
                column: "NoOfDays",
                value: 1);

            migrationBuilder.UpdateData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 7,
                column: "NoOfDays",
                value: 1);

            migrationBuilder.UpdateData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 8,
                column: "NoOfDays",
                value: 2);

            migrationBuilder.UpdateData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 9,
                column: "NoOfDays",
                value: 2);

            
        }
    }
}
