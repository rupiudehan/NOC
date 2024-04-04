using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class paymentpayerdetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PayerEmail",
                table: "GrantPaymentDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayerId",
                table: "GrantPaymentDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayerName",
                table: "GrantPaymentDetails",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayerEmail",
                table: "GrantPaymentDetails");

            migrationBuilder.DropColumn(
                name: "PayerId",
                table: "GrantPaymentDetails");

            migrationBuilder.DropColumn(
                name: "PayerName",
                table: "GrantPaymentDetails");
        }
    }
}
