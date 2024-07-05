using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class DrainWidthTypeDetailsCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "DrainWidthTypeDetails",
                type: "text",
                nullable: true);

            

            migrationBuilder.UpdateData(
                table: "DrainWidthTypeDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: "N");

            migrationBuilder.UpdateData(
                table: "DrainWidthTypeDetails",
                keyColumn: "Id",
                keyValue: 2,
                column: "Code",
                value: "C");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "DrainWidthTypeDetails");

        }
    }
}
