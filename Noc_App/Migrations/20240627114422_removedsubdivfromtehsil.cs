using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class removedsubdivfromtehsil : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TehsilBlockDetails_SubDivisionDetails_SubDivisionId",
                table: "TehsilBlockDetails");

            migrationBuilder.DropIndex(
                name: "IX_TehsilBlockDetails_SubDivisionId",
                table: "TehsilBlockDetails");

            migrationBuilder.DropColumn(
                name: "SubDivisionId",
                table: "TehsilBlockDetails");

            migrationBuilder.AddColumn<int>(
                name: "SubDivisionDetailsId",
                table: "TehsilBlockDetails",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubDivisionId",
                table: "GrantDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            

            migrationBuilder.CreateIndex(
                name: "IX_TehsilBlockDetails_SubDivisionDetailsId",
                table: "TehsilBlockDetails",
                column: "SubDivisionDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_GrantDetails_SubDivisionId",
                table: "GrantDetails",
                column: "SubDivisionId");

            migrationBuilder.AddForeignKey(
                name: "FK_GrantDetails_SubDivisionDetails_SubDivisionId",
                table: "GrantDetails",
                column: "SubDivisionId",
                principalTable: "SubDivisionDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TehsilBlockDetails_SubDivisionDetails_SubDivisionDetailsId",
                table: "TehsilBlockDetails",
                column: "SubDivisionDetailsId",
                principalTable: "SubDivisionDetails",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GrantDetails_SubDivisionDetails_SubDivisionId",
                table: "GrantDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TehsilBlockDetails_SubDivisionDetails_SubDivisionDetailsId",
                table: "TehsilBlockDetails");

            migrationBuilder.DropIndex(
                name: "IX_TehsilBlockDetails_SubDivisionDetailsId",
                table: "TehsilBlockDetails");

            migrationBuilder.DropIndex(
                name: "IX_GrantDetails_SubDivisionId",
                table: "GrantDetails");

            migrationBuilder.DropColumn(
                name: "SubDivisionDetailsId",
                table: "TehsilBlockDetails");

            migrationBuilder.DropColumn(
                name: "SubDivisionId",
                table: "GrantDetails");

            migrationBuilder.AddColumn<int>(
                name: "SubDivisionId",
                table: "TehsilBlockDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            

            migrationBuilder.CreateIndex(
                name: "IX_TehsilBlockDetails_SubDivisionId",
                table: "TehsilBlockDetails",
                column: "SubDivisionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TehsilBlockDetails_SubDivisionDetails_SubDivisionId",
                table: "TehsilBlockDetails",
                column: "SubDivisionId",
                principalTable: "SubDivisionDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
