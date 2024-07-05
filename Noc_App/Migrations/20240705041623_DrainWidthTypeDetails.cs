using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class DrainWidthTypeDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DrainWidth",
                table: "GrantApprovalProcessDocumentsDetails",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "IsDrainNotified",
                table: "GrantApprovalProcessDocumentsDetails",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "TypeOfWidth",
                table: "GrantApprovalProcessDocumentsDetails",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "DrainWidthTypeDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrainWidthTypeDetails", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "DrainWidthTypeDetails",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "As Per Notification" },
                    { 2, "As Per Calculation" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GrantApprovalProcessDocumentsDetails_TypeOfWidth",
                table: "GrantApprovalProcessDocumentsDetails",
                column: "TypeOfWidth");

            migrationBuilder.AddForeignKey(
                name: "FK_GrantApprovalProcessDocumentsDetails_DrainWidthTypeDetails_~",
                table: "GrantApprovalProcessDocumentsDetails",
                column: "TypeOfWidth",
                principalTable: "DrainWidthTypeDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GrantApprovalProcessDocumentsDetails_DrainWidthTypeDetails_~",
                table: "GrantApprovalProcessDocumentsDetails");

            migrationBuilder.DropTable(
                name: "DrainWidthTypeDetails");

            migrationBuilder.DropIndex(
                name: "IX_GrantApprovalProcessDocumentsDetails_TypeOfWidth",
                table: "GrantApprovalProcessDocumentsDetails");

            migrationBuilder.DropColumn(
                name: "DrainWidth",
                table: "GrantApprovalProcessDocumentsDetails");

            migrationBuilder.DropColumn(
                name: "IsDrainNotified",
                table: "GrantApprovalProcessDocumentsDetails");

            migrationBuilder.DropColumn(
                name: "TypeOfWidth",
                table: "GrantApprovalProcessDocumentsDetails");
            
        }
    }
}
