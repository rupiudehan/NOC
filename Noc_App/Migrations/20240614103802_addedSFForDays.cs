using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class addedSFForDays : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DaysCheckMaster",
                columns: new[] { "Id", "CheckFor", "Code", "IsRelatedToForward", "IsRelatedToIssue", "NoOfDays", "UserRoleID" },
                values: new object[] { 12, "Shortfall", "SF", 0, 0, 7.0, 7 });

            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 12);

            
        }
    }
}
