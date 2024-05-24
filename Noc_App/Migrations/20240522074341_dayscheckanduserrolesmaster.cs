using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class dayscheckanduserrolesmaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "DaysCheckMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CheckFor = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    IsRelatedToForward = table.Column<int>(type: "integer", nullable: false),
                    IsRelatedToIssue = table.Column<int>(type: "integer", nullable: false),
                    NoOfDays = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DaysCheckMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoleDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleName = table.Column<string>(type: "text", nullable: true),
                    AppRoleName = table.Column<string>(type: "text", nullable: true),
                    RoleLevel = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleDetails", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "DaysCheckMaster",
                columns: new[] { "Id", "CheckFor", "Code", "IsRelatedToForward", "IsRelatedToIssue", "NoOfDays" },
                values: new object[,]
                {
                    { 1, "Executive Engineer", "EEF", 1, 0, 1 },
                    { 2, "Chief Engineer", "CEHQF", 1, 0, 3 },
                    { 3, "Junior Engineer", "JE", 1, 0, 2 },
                    { 4, "Sub Divisional Officer", "SDO", 1, 0, 2 },
                    { 5, "XEN HO Drainage", "EEHQ", 0, 1, 3 },
                    { 6, "XEN/DWS", "D", 1, 0, 1 },
                    { 7, "Principal Secretary", "PS", 1, 0, 1 },
                    { 8, "Superintending Engineer", "CO", 1, 0, 2 },
                    { 9, "Executive Engineer", "EES", 0, 1, 2 }
                });

            migrationBuilder.InsertData(
                table: "UserRoleDetails",
                columns: new[] { "Id", "AppRoleName", "RoleLevel", "RoleName" },
                values: new object[,]
                {
                    { 1, "Administrator", 1, "Administrator" },
                    { 6, "PRINCIPAL SECRETARY", 2, "Principal Secretary" },
                    { 7, "EXECUTIVE ENGINEER", 7, "Executive Engineer" },
                    { 8, "CIRCLE OFFICER", 6, "Superintending Engineer" },
                    { 10, "CHIEF ENGINEER HQ", 3, "Chief Engineer" },
                    { 60, "JUNIOR ENGINEER", 9, "Junior Engineer" },
                    { 67, "SUB DIVISIONAL OFFICER", 8, "Sub Divisional Officer" },
                    { 83, "DWS", 5, "XEN/DWS" },
                    { 128, "EXECUTIVE ENGINEER HQ", 4, "XEN HO Drainage" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "DaysCheckMaster");

            migrationBuilder.DropTable(
                name: "UserRoleDetails");
        }
    }
}
