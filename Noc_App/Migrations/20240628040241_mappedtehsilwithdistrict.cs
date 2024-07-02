using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class mappedtehsilwithdistrict : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TehsilBlockDetails_SubDivisionDetails_SubDivisionDetailsId",
                table: "TehsilBlockDetails");

            migrationBuilder.DropIndex(
                name: "IX_TehsilBlockDetails_SubDivisionDetailsId",
                table: "TehsilBlockDetails");

            migrationBuilder.DropColumn(
                name: "SubDivisionDetailsId",
                table: "TehsilBlockDetails");

            migrationBuilder.AddColumn<int>(
                name: "DistrictId",
                table: "TehsilBlockDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            

            migrationBuilder.CreateIndex(
                name: "IX_TehsilBlockDetails_DistrictId",
                table: "TehsilBlockDetails",
                column: "DistrictId");

            migrationBuilder.AddForeignKey(
                name: "FK_TehsilBlockDetails_DistrictDetails_DistrictId",
                table: "TehsilBlockDetails",
                column: "DistrictId",
                principalTable: "DistrictDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TehsilBlockDetails_DistrictDetails_DistrictId",
                table: "TehsilBlockDetails");

            migrationBuilder.DropIndex(
                name: "IX_TehsilBlockDetails_DistrictId",
                table: "TehsilBlockDetails");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "TehsilBlockDetails");

            migrationBuilder.AddColumn<int>(
                name: "SubDivisionDetailsId",
                table: "TehsilBlockDetails",
                type: "integer",
                nullable: true);

            
            migrationBuilder.CreateIndex(
                name: "IX_TehsilBlockDetails_SubDivisionDetailsId",
                table: "TehsilBlockDetails",
                column: "SubDivisionDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_TehsilBlockDetails_SubDivisionDetails_SubDivisionDetailsId",
                table: "TehsilBlockDetails",
                column: "SubDivisionDetailsId",
                principalTable: "SubDivisionDetails",
                principalColumn: "Id");
        }
    }
}
