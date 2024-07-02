using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class modifieddivision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "Ddocode",
                table: "DivisionDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayLocatioinCode",
                table: "DivisionDetails",
                type: "text",
                nullable: true);

            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "Ddocode",
                table: "DivisionDetails");

            migrationBuilder.DropColumn(
                name: "PayLocatioinCode",
                table: "DivisionDetails");

           
        }
    }
}
