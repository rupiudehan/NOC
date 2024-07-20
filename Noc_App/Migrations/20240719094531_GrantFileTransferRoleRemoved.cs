using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class GrantFileTransferRoleRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromAuthorityRole",
                table: "GrantFileTransferDetails");

            migrationBuilder.DropColumn(
                name: "ToAuthorityRole",
                table: "GrantFileTransferDetails");

            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FromAuthorityRole",
                table: "GrantFileTransferDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ToAuthorityRole",
                table: "GrantFileTransferDetails",
                type: "text",
                nullable: true);

            
        }
    }
}
