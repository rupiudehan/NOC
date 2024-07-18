using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class removedvillageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GrantDetails_VillageDetails_VillageID",
                table: "GrantDetails");

            migrationBuilder.DropTable(
                name: "VillageDetails");

            migrationBuilder.RenameColumn(
                name: "VillageID",
                table: "GrantDetails",
                newName: "TehsilID");

            migrationBuilder.RenameIndex(
                name: "IX_GrantDetails_VillageID",
                table: "GrantDetails",
                newName: "IX_GrantDetails_TehsilID");

            

            migrationBuilder.AddColumn<string>(
                name: "PinCode",
                table: "GrantDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VillageName",
                table: "GrantDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GrantDetails_TehsilBlockDetails_TehsilID",
                table: "GrantDetails",
                column: "TehsilID",
                principalTable: "TehsilBlockDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GrantDetails_TehsilBlockDetails_TehsilID",
                table: "GrantDetails");

           
            migrationBuilder.DropColumn(
                name: "PinCode",
                table: "GrantDetails");

            migrationBuilder.DropColumn(
                name: "VillageName",
                table: "GrantDetails");

            migrationBuilder.RenameColumn(
                name: "TehsilID",
                table: "GrantDetails",
                newName: "VillageID");

            migrationBuilder.RenameIndex(
                name: "IX_GrantDetails_TehsilID",
                table: "GrantDetails",
                newName: "IX_GrantDetails_VillageID");

            migrationBuilder.CreateTable(
                name: "VillageDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TehsilBlockId = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    PinCode = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VillageDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VillageDetails_TehsilBlockDetails_TehsilBlockId",
                        column: x => x.TehsilBlockId,
                        principalTable: "TehsilBlockDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            

            migrationBuilder.CreateIndex(
                name: "IX_VillageDetails_TehsilBlockId",
                table: "VillageDetails",
                column: "TehsilBlockId");

            migrationBuilder.AddForeignKey(
                name: "FK_GrantDetails_VillageDetails_VillageID",
                table: "GrantDetails",
                column: "VillageID",
                principalTable: "VillageDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
