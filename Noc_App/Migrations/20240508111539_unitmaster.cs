using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class unitmaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SiteUnitMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UnitName = table.Column<string>(type: "text", nullable: true),
                    UnitCode = table.Column<string>(type: "text", nullable: true),
                    UnitValue = table.Column<double>(type: "double precision", nullable: false),
                    Timesof = table.Column<double>(type: "double precision", nullable: false),
                    DivideBy = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteUnitMaster", x => x.Id);
                });



            migrationBuilder.InsertData(
                table: "SiteUnitMaster",
                columns: new[] { "Id", "DivideBy", "Timesof", "UnitCode", "UnitName", "UnitValue" },
                values: new object[,]
                {
                    { 1, 1.0, 1.0, "BSI", "Biswa-I", 0.012500000000000001 },
                    { 2, 1.0, 1.0, "BGI", "Bigha-I", 0.25 },
                    { 3, 1.0, 1.0, "BWI", "Biswansi-I", 0.00062500000000000001 },
                    { 4, 1.0, 3.0, "BSI", "Biswa-II", 0.012500000000000001 },
                    { 5, 1.0, 3.0, "BGI", "Bigha-II", 0.25 },
                    { 6, 1.0, 3.0, "BWI", "Biswansi-II", 0.00062500000000000001 },
                    { 7, 160.0, 1.0, "K", "Kanal", 1.0 },
                    { 8, 8.0, 1.0, "K", "Marla", 1.0 },
                    { 9, 1440.0, 1.0, "K", "Sarsai", 1.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiteUnitMaster");

        }
    }
}
