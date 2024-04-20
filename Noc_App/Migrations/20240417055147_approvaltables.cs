using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class approvaltables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GrantApprovalDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GrantID = table.Column<int>(type: "integer", nullable: false),
                    ApprovalID = table.Column<int>(type: "integer", nullable: false),
                    ProcessLevel = table.Column<int>(type: "integer", nullable: false),
                    ProcessedToUser = table.Column<string>(type: "text", nullable: true),
                    ProcessedToRole = table.Column<string>(type: "text", nullable: true),
                    ProcessedBy = table.Column<string>(type: "text", nullable: true),
                    ProcessedByRole = table.Column<string>(type: "text", nullable: true),
                    ProcessedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrantApprovalDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrantApprovalDetails_GrantApprovalMaster_ApprovalID",
                        column: x => x.ApprovalID,
                        principalTable: "GrantApprovalMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GrantApprovalDetails_GrantDetails_GrantID",
                        column: x => x.GrantID,
                        principalTable: "GrantDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GrantApprovalProcessDocumentsDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GrantApprovalID = table.Column<long>(type: "bigint", nullable: false),
                    SiteConditionReportPath = table.Column<string>(type: "text", nullable: true),
                    CatchmentAreaAndFlowPath = table.Column<string>(type: "text", nullable: true),
                    DistanceFromCreekPath = table.Column<string>(type: "text", nullable: true),
                    GISOrDWSReportPath = table.Column<string>(type: "text", nullable: true),
                    KmlFileVerificationReportPath = table.Column<string>(type: "text", nullable: true),
                    CrossSectionOrCalculationSheetReportPath = table.Column<string>(type: "text", nullable: true),
                    DrainLSectionPath = table.Column<string>(type: "text", nullable: true),
                    ProcessedBy = table.Column<string>(type: "text", nullable: true),
                    ProcessedByRole = table.Column<string>(type: "text", nullable: true),
                    ProcessedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedByRole = table.Column<string>(type: "text", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrantApprovalProcessDocumentsDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrantApprovalProcessDocumentsDetails_GrantApprovalDetails_G~",
                        column: x => x.GrantApprovalID,
                        principalTable: "GrantApprovalDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "SiteAreaUnitDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Bigha/Biswa/Biswansi");

            migrationBuilder.UpdateData(
                table: "SiteAreaUnitDetails",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Kanal/Marla/Sarsai");

            migrationBuilder.CreateIndex(
                name: "IX_GrantApprovalDetails_ApprovalID",
                table: "GrantApprovalDetails",
                column: "ApprovalID");

            migrationBuilder.CreateIndex(
                name: "IX_GrantApprovalDetails_GrantID",
                table: "GrantApprovalDetails",
                column: "GrantID");

            migrationBuilder.CreateIndex(
                name: "IX_GrantApprovalProcessDocumentsDetails_GrantApprovalID",
                table: "GrantApprovalProcessDocumentsDetails",
                column: "GrantApprovalID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GrantApprovalProcessDocumentsDetails");

            migrationBuilder.DropTable(
                name: "GrantApprovalDetails");

            migrationBuilder.UpdateData(
                table: "SiteAreaUnitDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Biswansi/Biswa/Bigha");

            migrationBuilder.UpdateData(
                table: "SiteAreaUnitDetails",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Marla/Kanal/Sarsai");
        }
    }
}
