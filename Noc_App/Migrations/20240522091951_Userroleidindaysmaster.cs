using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class Userroleidindaysmaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserRoleID",
                table: "DaysCheckMaster",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 1,
                column: "UserRoleID",
                value: 7);

            migrationBuilder.UpdateData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 2,
                column: "UserRoleID",
                value: 10);

            migrationBuilder.UpdateData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 3,
                column: "UserRoleID",
                value: 60);

            migrationBuilder.UpdateData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 4,
                column: "UserRoleID",
                value: 67);

            migrationBuilder.UpdateData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 5,
                column: "UserRoleID",
                value: 128);

            migrationBuilder.UpdateData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 6,
                column: "UserRoleID",
                value: 83);

            migrationBuilder.UpdateData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 7,
                column: "UserRoleID",
                value: 6);

            migrationBuilder.UpdateData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 8,
                column: "UserRoleID",
                value: 8);

            migrationBuilder.UpdateData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 9,
                column: "UserRoleID",
                value: 7);

            migrationBuilder.CreateIndex(
                name: "IX_DaysCheckMaster_UserRoleID",
                table: "DaysCheckMaster",
                column: "UserRoleID");

            migrationBuilder.AddForeignKey(
                name: "FK_DaysCheckMaster_UserRoleDetails_UserRoleID",
                table: "DaysCheckMaster",
                column: "UserRoleID",
                principalTable: "UserRoleDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DaysCheckMaster_UserRoleDetails_UserRoleID",
                table: "DaysCheckMaster");

            migrationBuilder.DropIndex(
                name: "IX_DaysCheckMaster_UserRoleID",
                table: "DaysCheckMaster");

            migrationBuilder.DropColumn(
                name: "UserRoleID",
                table: "DaysCheckMaster");
        }
    }
}
