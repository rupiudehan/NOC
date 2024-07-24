using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class masterplandetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUnderMasterPlan",
                table: "GrantDetails",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MasterPlanId",
                table: "GrantDetails",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MasterPlanDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MainPlanName = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    DistrictId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterPlanDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterPlanDetails_DistrictDetails_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "DistrictDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            

            migrationBuilder.InsertData(
                table: "MasterPlanDetails",
                columns: new[] { "Id", "Code", "DistrictId", "MainPlanName", "Name" },
                values: new object[,]
                {
                    { 1, "B", 19, "GMADA", "Banur (2031)" },
                    { 2, "D", 19, "GMADA", "Dera Bassi" },
                    { 3, "F", 19, "GMADA", "FatehGarh Sahib (2010-2031)" },
                    { 4, "G", 19, "GMADA", "GMADA Regional Plan" },
                    { 5, "K", 19, "GMADA", "Kharar (2031)" },
                    { 6, "L", 19, "GMADA", "Lalru" },
                    { 7, "D", 19, "GMADA", "Dera Bassi" },
                    { 8, "MG", 19, "GMADA", "Mandigobindgarh (2010-2031)" },
                    { 9, "M", 19, "GMADA", "Mullanpur" },
                    { 10, "NC", 19, "GMADA", "New Chandigarh (2008-2031)" },
                    { 11, "Z", 19, "GMADA", "Zirakpur (2031)" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GrantDetails_MasterPlanId",
                table: "GrantDetails",
                column: "MasterPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterPlanDetails_DistrictId",
                table: "MasterPlanDetails",
                column: "DistrictId");

            migrationBuilder.AddForeignKey(
                name: "FK_GrantDetails_MasterPlanDetails_MasterPlanId",
                table: "GrantDetails",
                column: "MasterPlanId",
                principalTable: "MasterPlanDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GrantDetails_MasterPlanDetails_MasterPlanId",
                table: "GrantDetails");

            migrationBuilder.DropTable(
                name: "MasterPlanDetails");

            migrationBuilder.DropIndex(
                name: "IX_GrantDetails_MasterPlanId",
                table: "GrantDetails");

            migrationBuilder.DropColumn(
                name: "IsUnderMasterPlan",
                table: "GrantDetails");

            migrationBuilder.DropColumn(
                name: "MasterPlanId",
                table: "GrantDetails");

            
        }
    }
}
