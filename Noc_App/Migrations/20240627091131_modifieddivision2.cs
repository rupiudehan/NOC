using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class modifieddivision2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ddocode",
                table: "DivisionDetails",
                newName: "DdoCode");

            migrationBuilder.RenameColumn(
                name: "PayLocatioinCode",
                table: "DivisionDetails",
                newName: "PayLocationCode");

           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DdoCode",
                table: "DivisionDetails",
                newName: "Ddocode");

            migrationBuilder.RenameColumn(
                name: "PayLocationCode",
                table: "DivisionDetails",
                newName: "PayLocatioinCode");

            
        }
    }
}
