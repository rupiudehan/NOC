using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class rejectremarks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GrantApprovalProcessDocumentsDetails_GrantApprovalID",
                table: "GrantApprovalProcessDocumentsDetails");

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "GrantApprovalDetails",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GrantApprovalProcessDocumentsDetails_GrantApprovalID",
                table: "GrantApprovalProcessDocumentsDetails",
                column: "GrantApprovalID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GrantApprovalProcessDocumentsDetails_GrantApprovalID",
                table: "GrantApprovalProcessDocumentsDetails");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "GrantApprovalDetails");

            migrationBuilder.CreateIndex(
                name: "IX_GrantApprovalProcessDocumentsDetails_GrantApprovalID",
                table: "GrantApprovalProcessDocumentsDetails",
                column: "GrantApprovalID");
        }
    }
}
