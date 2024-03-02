using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedDrainCoordinatesAndDrainDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DrainDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrainDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DrainDetails_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DrainDetails_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DrainCoordinatesDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Latitude = table.Column<decimal>(type: "numeric", nullable: false),
                    Longitude = table.Column<decimal>(type: "numeric", nullable: false),
                    DrainId = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrainCoordinatesDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DrainCoordinatesDetails_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DrainCoordinatesDetails_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DrainCoordinatesDetails_DrainDetails_DrainId",
                        column: x => x.DrainId,
                        principalTable: "DrainDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DrainCoordinatesDetails_CreatedBy",
                table: "DrainCoordinatesDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DrainCoordinatesDetails_DrainId",
                table: "DrainCoordinatesDetails",
                column: "DrainId");

            migrationBuilder.CreateIndex(
                name: "IX_DrainCoordinatesDetails_UpdatedBy",
                table: "DrainCoordinatesDetails",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DrainDetails_CreatedBy",
                table: "DrainDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DrainDetails_UpdatedBy",
                table: "DrainDetails",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrainCoordinatesDetails");

            migrationBuilder.DropTable(
                name: "DrainDetails");
        }
    }
}
