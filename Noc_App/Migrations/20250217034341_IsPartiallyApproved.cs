using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class IsPartiallyApproved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPartiallyApproved",
                table: "GrantDetails",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPartiallyApproved",
                table: "GrantDetails");
        }
    }
}
