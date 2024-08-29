using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class establishmentOffice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "EstablishmentOfficeDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstablishmentOfficeDetails", x => x.Id);
                });


            migrationBuilder.InsertData(
                table: "EstablishmentOfficeDetails",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 3, "CHIEF ENGINEER HEAD QUARTER" },
                    { 37, "CHIEF ENGINEER DESIGN CHANDIGARH" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstablishmentOfficeDetails");

        }
    }
}
