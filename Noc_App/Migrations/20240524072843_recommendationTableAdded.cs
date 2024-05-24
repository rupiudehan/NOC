using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class recommendationTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "RecommendationID",
                table: "GrantApprovalDetails",
                type: "integer",
                nullable: false,
                defaultValue: 3);

            migrationBuilder.CreateTable(
                name: "RecommendationDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecommendationDetail", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "IsRelatedToForward", "IsRelatedToIssue" },
                values: new object[] { 1, 0 });

            migrationBuilder.InsertData(
                table: "RecommendationDetail",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { 1, "A", "Approved" },
                    { 2, "R", "Rejected" },
                    { 3, "NA", "Nothing" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GrantApprovalDetails_RecommendationID",
                table: "GrantApprovalDetails",
                column: "RecommendationID");

            migrationBuilder.AddForeignKey(
                name: "FK_GrantApprovalDetails_RecommendationDetail_RecommendationID",
                table: "GrantApprovalDetails",
                column: "RecommendationID",
                principalTable: "RecommendationDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GrantApprovalDetails_RecommendationDetail_RecommendationID",
                table: "GrantApprovalDetails");

            migrationBuilder.DropTable(
                name: "RecommendationDetail");

            migrationBuilder.DropIndex(
                name: "IX_GrantApprovalDetails_RecommendationID",
                table: "GrantApprovalDetails");

            migrationBuilder.DropColumn(
                name: "RecommendationID",
                table: "GrantApprovalDetails");


            migrationBuilder.UpdateData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "IsRelatedToForward", "IsRelatedToIssue" },
                values: new object[] { 0, 1 });
        }
    }
}
