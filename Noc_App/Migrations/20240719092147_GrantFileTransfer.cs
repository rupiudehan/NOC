using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class GrantFileTransfer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GrantFileTransferDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GrantId = table.Column<int>(type: "integer", nullable: false),
                    FromAuthorityId = table.Column<string>(type: "text", nullable: true),
                    FromAuthorityRole = table.Column<string>(type: "text", nullable: true),
                    ToAuthorityId = table.Column<string>(type: "text", nullable: true),
                    ToAuthorityRole = table.Column<string>(type: "text", nullable: true),
                    TransferedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrantFileTransferDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrantFileTransferDetails_GrantDetails_GrantId",
                        column: x => x.GrantId,
                        principalTable: "GrantDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GrantFileTransferDetails_GrantId",
                table: "GrantFileTransferDetails",
                column: "GrantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GrantFileTransferDetails");

            
        }
    }
}
