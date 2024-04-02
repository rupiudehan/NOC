using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class payment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GrantPaymentDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GrantID = table.Column<int>(type: "integer", nullable: false),
                    referenceId = table.Column<string>(type: "text", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    paymentid = table.Column<int>(type: "integer", nullable: false),
                    sessionid = table.Column<string>(type: "text", nullable: true),
                    Paymentstatus = table.Column<string>(type: "text", nullable: true),
                    PaymentOrderId = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrantPaymentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrantPaymentDetails_GrantDetails_GrantID",
                        column: x => x.GrantID,
                        principalTable: "GrantDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GrantPaymentDetails_GrantID",
                table: "GrantPaymentDetails",
                column: "GrantID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GrantPaymentDetails");
        }
    }
}
