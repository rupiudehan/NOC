using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class seedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "NocPermissionTypeDetails",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Residential" },
                    { 2, "Industrial" },
                    { 3, "Commercial" }
                });

            migrationBuilder.InsertData(
                table: "NocTypeDetails",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "New" },
                    { 2, "Extension of Existing Project" }
                });

            migrationBuilder.InsertData(
                table: "OwnerTypeDetails",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Owner" },
                    { 2, "Partners" },
                    { 3, "Chief Executive" },
                    { 4, "Full Time Directors" }
                });

            migrationBuilder.InsertData(
                table: "ProjectTypeDetails",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Residentials" },
                    { 2, "Industrial" },
                    { 3, "Commercial" },
                    { 4, "Any Other" }
                });

            migrationBuilder.InsertData(
                table: "SiteAreaUnitDetails",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Bigha/Biswa/Biswansi" },
                    { 2, "Kanal/Marla/Sarsai" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NocPermissionTypeDetails",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "NocPermissionTypeDetails",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "NocPermissionTypeDetails",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "NocTypeDetails",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "NocTypeDetails",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OwnerTypeDetails",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OwnerTypeDetails",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OwnerTypeDetails",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OwnerTypeDetails",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProjectTypeDetails",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProjectTypeDetails",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProjectTypeDetails",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProjectTypeDetails",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "SiteAreaUnitDetails",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SiteAreaUnitDetails",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
