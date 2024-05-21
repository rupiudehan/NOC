using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class addchallandetailsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "paymentid",
                table: "GrantPaymentDetails",
                newName: "deptRefNo");

            migrationBuilder.RenameColumn(
                name: "Paymentstatus",
                table: "GrantPaymentDetails",
                newName: "PayerName");

            migrationBuilder.AddColumn<string>(
                name: "PayerEmail",
                table: "GrantPaymentDetails",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChallanDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    deptRefNo = table.Column<string>(type: "text", nullable: true),
                    challanDate = table.Column<string>(type: "text", nullable: true),
                    expiryDate = table.Column<string>(type: "text", nullable: true),
                    companyName = table.Column<string>(type: "text", nullable: true),
                    deptCode = table.Column<string>(type: "text", nullable: true),
                    totalAmt = table.Column<string>(type: "text", nullable: true),
                    trsyAmt = table.Column<string>(type: "text", nullable: true),
                    nonTrsyAmt = table.Column<string>(type: "text", nullable: true),
                    noOfTrans = table.Column<string>(type: "text", nullable: true),
                    ddoCode = table.Column<string>(type: "text", nullable: true),
                    payLocCode = table.Column<string>(type: "text", nullable: true),
                    add1 = table.Column<string>(type: "text", nullable: true),
                    add2 = table.Column<string>(type: "text", nullable: true),
                    add3 = table.Column<string>(type: "text", nullable: true),
                    add4 = table.Column<string>(type: "text", nullable: true),
                    add5 = table.Column<string>(type: "text", nullable: true),
                    sURL = table.Column<string>(type: "text", nullable: true),
                    fURL = table.Column<string>(type: "text", nullable: true),
                    payerName = table.Column<string>(type: "text", nullable: true),
                    teleNumber = table.Column<string>(type: "text", nullable: true),
                    mobNumber = table.Column<string>(type: "text", nullable: true),
                    emailId = table.Column<string>(type: "text", nullable: true),
                    addLine1 = table.Column<string>(type: "text", nullable: true),
                    addLine2 = table.Column<string>(type: "text", nullable: true),
                    addPincode = table.Column<string>(type: "text", nullable: true),
                    district = table.Column<string>(type: "text", nullable: true),
                    tehsil = table.Column<string>(type: "text", nullable: true),
                    ApplicationId = table.Column<string>(type: "text", nullable: true),
                    RequestStatus = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallanDetails", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChallanDetails");

            migrationBuilder.DropColumn(
                name: "PayerEmail",
                table: "GrantPaymentDetails");

            migrationBuilder.RenameColumn(
                name: "deptRefNo",
                table: "GrantPaymentDetails",
                newName: "paymentid");

            migrationBuilder.RenameColumn(
                name: "PayerName",
                table: "GrantPaymentDetails",
                newName: "Paymentstatus");
        }
    }
}
