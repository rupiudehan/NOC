using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class linkedtehsilwithdivision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GrantApprovalProcessDocumentsDetails_DrainWidthTypeDetails_~",
                table: "GrantApprovalProcessDocumentsDetails");

            migrationBuilder.RenameColumn(
                name: "DistrictId",
                table: "TehsilBlockDetails",
                newName: "DivisiontId");

            migrationBuilder.RenameIndex(
                name: "IX_TehsilBlockDetails_DistrictId",
                table: "TehsilBlockDetails",
                newName: "IX_TehsilBlockDetails_DivisiontId");

            migrationBuilder.AlterColumn<int>(
                name: "TypeOfWidth",
                table: "GrantApprovalProcessDocumentsDetails",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            

            migrationBuilder.AddForeignKey(
                name: "FK_GrantApprovalProcessDocumentsDetails_DrainWidthTypeDetails_~",
                table: "GrantApprovalProcessDocumentsDetails",
                column: "TypeOfWidth",
                principalTable: "DrainWidthTypeDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TehsilBlockDetails_DivisionDetails_DivisiontId",
                table: "TehsilBlockDetails",
                column: "DivisiontId",
                principalTable: "DivisionDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GrantApprovalProcessDocumentsDetails_DrainWidthTypeDetails_~",
                table: "GrantApprovalProcessDocumentsDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TehsilBlockDetails_DivisionDetails_DivisiontId",
                table: "TehsilBlockDetails");

            migrationBuilder.RenameColumn(
                name: "DivisiontId",
                table: "TehsilBlockDetails",
                newName: "DistrictId");

            migrationBuilder.RenameIndex(
                name: "IX_TehsilBlockDetails_DivisiontId",
                table: "TehsilBlockDetails",
                newName: "IX_TehsilBlockDetails_DistrictId");

            migrationBuilder.AlterColumn<int>(
                name: "TypeOfWidth",
                table: "GrantApprovalProcessDocumentsDetails",
                type: "integer",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            

            migrationBuilder.AddForeignKey(
                name: "FK_GrantApprovalProcessDocumentsDetails_DrainWidthTypeDetails_~",
                table: "GrantApprovalProcessDocumentsDetails",
                column: "TypeOfWidth",
                principalTable: "DrainWidthTypeDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TehsilBlockDetails_DistrictDetails_DistrictId",
                table: "TehsilBlockDetails",
                column: "DistrictId",
                principalTable: "DistrictDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
