using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class isundermasterplanindocs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MasterPlanDetails",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.AddColumn<bool>(
                name: "IsUnderMasterPlan",
                table: "GrantApprovalProcessDocumentsDetails",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            

            migrationBuilder.UpdateData(
                table: "MasterPlanDetails",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Code", "Name" },
                values: new object[] { "MG", "Mandigobindgarh (2010-2031)" });

            migrationBuilder.UpdateData(
                table: "MasterPlanDetails",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Code", "Name" },
                values: new object[] { "M", "Mullanpur" });

            migrationBuilder.UpdateData(
                table: "MasterPlanDetails",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Code", "Name" },
                values: new object[] { "NC", "New Chandigarh (2008-2031)" });

            migrationBuilder.UpdateData(
                table: "MasterPlanDetails",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Code", "Name" },
                values: new object[] { "Z", "Zirakpur (2031)" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUnderMasterPlan",
                table: "GrantApprovalProcessDocumentsDetails");

            

            migrationBuilder.UpdateData(
                table: "MasterPlanDetails",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Code", "Name" },
                values: new object[] { "D", "Dera Bassi" });

            migrationBuilder.UpdateData(
                table: "MasterPlanDetails",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Code", "Name" },
                values: new object[] { "MG", "Mandigobindgarh (2010-2031)" });

            migrationBuilder.UpdateData(
                table: "MasterPlanDetails",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Code", "Name" },
                values: new object[] { "M", "Mullanpur" });

            migrationBuilder.UpdateData(
                table: "MasterPlanDetails",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Code", "Name" },
                values: new object[] { "NC", "New Chandigarh (2008-2031)" });

            migrationBuilder.InsertData(
                table: "MasterPlanDetails",
                columns: new[] { "Id", "Code", "DistrictId", "MainPlanName", "Name" },
                values: new object[] { 11, "Z", 19, "GMADA", "Zirakpur (2031)" });
        }
    }
}
