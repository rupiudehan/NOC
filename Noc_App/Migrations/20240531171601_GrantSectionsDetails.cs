using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class GrantSectionsDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "GrantSectionsDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SectionName = table.Column<string>(type: "text", nullable: true),
                    SectionCode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrantSectionsDetails", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "GrantSectionsDetails",
                columns: new[] { "Id", "SectionCode", "SectionName" },
                values: new object[,]
                {
                    { 1, "P", "Project" },
                    { 2, "AD", "Address" },
                    { 3, "KH", "Khasra" },
                    { 4, "K", "KML" },
                    { 5, "AP", "Applicant" },
                    { 6, "OW", "Owner" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GrantSectionsDetails");

        }
    }
}
