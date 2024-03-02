using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class changedforeignkeyofDrain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrainCoordinatesDetails_DrainDetails_DrainId",
                table: "DrainCoordinatesDetails");

            migrationBuilder.AlterColumn<int>(
                name: "DrainId",
                table: "DrainCoordinatesDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DrainCoordinatesDetails_DrainDetails_DrainId",
                table: "DrainCoordinatesDetails",
                column: "DrainId",
                principalTable: "DrainDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrainCoordinatesDetails_DrainDetails_DrainId",
                table: "DrainCoordinatesDetails");

            migrationBuilder.AlterColumn<int>(
                name: "DrainId",
                table: "DrainCoordinatesDetails",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_DrainCoordinatesDetails_DrainDetails_DrainId",
                table: "DrainCoordinatesDetails",
                column: "DrainId",
                principalTable: "DrainDetails",
                principalColumn: "Id");
        }
    }
}
