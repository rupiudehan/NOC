using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class circlechanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "CircleDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CircleDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CircleDivisionMapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CircleId = table.Column<int>(type: "integer", nullable: false),
                    DivisionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CircleDivisionMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CircleDivisionMapping_CircleDetails_CircleId",
                        column: x => x.CircleId,
                        principalTable: "CircleDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CircleDivisionMapping_DivisionDetails_DivisionId",
                        column: x => x.DivisionId,
                        principalTable: "DivisionDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CircleDetails",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { 1, "P", "Superintending Engineer Patiala Drainage-cum-Mining and Geology, Circle, WRD, Punjab" },
                    { 2, "J", "Superintending Engineer Jalandhar Drainage-cum-Mining & Geology Circle, WRD, Punjab" },
                    { 3, "FE", "Superintending Engineer Ferozepur Drainage-cum-Mining and Geology, Circle, WRD, Punjab" },
                    { 4, "A", "Superintending Engineer Amritsar Drainage-cum-Mining & Geology Circle, WRD Punjab" },
                    { 5, "R", "Superintending Engineer Ropar  Drainage-cum-Mining & Geology Circle, WRD Punjab" }
                });

            

            migrationBuilder.InsertData(
                table: "CircleDivisionMapping",
                columns: new[] { "Id", "CircleId", "DivisionId" },
                values: new object[,]
                {
                    { 2, 1, 43 },
                    { 3, 1, 34 },
                    { 4, 1, 60 },
                    { 5, 1, 88 },
                    { 6, 2, 4 },
                    { 7, 2, 46 },
                    { 8, 2, 75 },
                    { 9, 3, 30 },
                    { 10, 3, 33 },
                    { 11, 3, 54 },
                    { 12, 3, 9 },
                    { 13, 4, 63 },
                    { 14, 4, 81 },
                    { 15, 4, 39 },
                    { 16, 4, 125 },
                    { 17, 4, 187 },
                    { 18, 5, 19 },
                    { 19, 5, 27 },
                    { 20, 5, 14 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CircleDivisionMapping_CircleId",
                table: "CircleDivisionMapping",
                column: "CircleId");

            migrationBuilder.CreateIndex(
                name: "IX_CircleDivisionMapping_DivisionId",
                table: "CircleDivisionMapping",
                column: "DivisionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CircleDivisionMapping");

            migrationBuilder.DropTable(
                name: "CircleDetails");

           
        }
    }
}
