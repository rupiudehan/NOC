using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class changedrecommendationentries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.UpdateData(
                table: "RecommendationDetail",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "For Approval");

            migrationBuilder.UpdateData(
                table: "RecommendationDetail",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "For Rejection");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.UpdateData(
                table: "RecommendationDetail",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Approved");

            migrationBuilder.UpdateData(
                table: "RecommendationDetail",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Rejected");
        }
    }
}
