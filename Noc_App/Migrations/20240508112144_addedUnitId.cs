using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class addedUnitId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SiteAreaUnitId",
                table: "SiteUnitMaster",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 1,
                column: "SiteAreaUnitId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 2,
                column: "SiteAreaUnitId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 3,
                column: "SiteAreaUnitId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 4,
                column: "SiteAreaUnitId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 5,
                column: "SiteAreaUnitId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 6,
                column: "SiteAreaUnitId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 7,
                column: "SiteAreaUnitId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 8,
                column: "SiteAreaUnitId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "SiteUnitMaster",
                keyColumn: "Id",
                keyValue: 9,
                column: "SiteAreaUnitId",
                value: 2);

            migrationBuilder.CreateIndex(
                name: "IX_SiteUnitMaster_SiteAreaUnitId",
                table: "SiteUnitMaster",
                column: "SiteAreaUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_SiteUnitMaster_SiteAreaUnitDetails_SiteAreaUnitId",
                table: "SiteUnitMaster",
                column: "SiteAreaUnitId",
                principalTable: "SiteAreaUnitDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SiteUnitMaster_SiteAreaUnitDetails_SiteAreaUnitId",
                table: "SiteUnitMaster");

            migrationBuilder.DropIndex(
                name: "IX_SiteUnitMaster_SiteAreaUnitId",
                table: "SiteUnitMaster");

            migrationBuilder.DropColumn(
                name: "SiteAreaUnitId",
                table: "SiteUnitMaster");
        }
    }
}
