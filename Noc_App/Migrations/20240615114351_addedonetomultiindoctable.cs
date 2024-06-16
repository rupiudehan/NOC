using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class addedonetomultiindoctable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GrantApprovalProcessDocumentsDetails_GrantApprovalID",
                table: "GrantApprovalProcessDocumentsDetails");

            

            migrationBuilder.UpdateData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 12,
                column: "UserRoleID",
                value: 7);


            migrationBuilder.CreateIndex(
                name: "IX_GrantApprovalProcessDocumentsDetails_GrantApprovalID",
                table: "GrantApprovalProcessDocumentsDetails",
                column: "GrantApprovalID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GrantApprovalProcessDocumentsDetails_GrantApprovalID",
                table: "GrantApprovalProcessDocumentsDetails");


            migrationBuilder.UpdateData(
                table: "DaysCheckMaster",
                keyColumn: "Id",
                keyValue: 12,
                column: "UserRoleID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 2,
                column: "LGD_ID",
                value: 0);

            

            migrationBuilder.CreateIndex(
                name: "IX_GrantApprovalProcessDocumentsDetails_GrantApprovalID",
                table: "GrantApprovalProcessDocumentsDetails",
                column: "GrantApprovalID",
                unique: true);
        }
    }
}
