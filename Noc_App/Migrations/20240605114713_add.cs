using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GrantRejectionShortfallSection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GrantApprovalId = table.Column<long>(type: "bigint", nullable: false),
                    SectionId = table.Column<int>(type: "integer", nullable: false),
                    IsCompleted = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrantRejectionShortfallSection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrantRejectionShortfallSection_GrantApprovalDetails_GrantAp~",
                        column: x => x.GrantApprovalId,
                        principalTable: "GrantApprovalDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GrantRejectionShortfallSection_GrantSectionsDetails_Section~",
                        column: x => x.SectionId,
                        principalTable: "GrantSectionsDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GrantRejectionShortfallSection_GrantApprovalId",
                table: "GrantRejectionShortfallSection",
                column: "GrantApprovalId");

            migrationBuilder.CreateIndex(
                name: "IX_GrantRejectionShortfallSection_SectionId",
                table: "GrantRejectionShortfallSection",
                column: "SectionId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GrantRejectionShortfallSection");

        }
    }
}
