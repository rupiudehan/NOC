using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class multiplesections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GrantRejectionShortfallSection_GrantSectionsDetails_Section~",
                table: "GrantRejectionShortfallSection");

            migrationBuilder.DropIndex(
                name: "IX_GrantRejectionShortfallSection_SectionId",
                table: "GrantRejectionShortfallSection");

            migrationBuilder.AddColumn<int>(
                name: "SectionId",
                table: "GrantSectionsDetails",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "GrantSectionsDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "SectionId",
                value: null);

            migrationBuilder.UpdateData(
                table: "GrantSectionsDetails",
                keyColumn: "Id",
                keyValue: 2,
                column: "SectionId",
                value: null);

            migrationBuilder.UpdateData(
                table: "GrantSectionsDetails",
                keyColumn: "Id",
                keyValue: 3,
                column: "SectionId",
                value: null);

            migrationBuilder.UpdateData(
                table: "GrantSectionsDetails",
                keyColumn: "Id",
                keyValue: 4,
                column: "SectionId",
                value: null);

            migrationBuilder.UpdateData(
                table: "GrantSectionsDetails",
                keyColumn: "Id",
                keyValue: 5,
                column: "SectionId",
                value: null);

            migrationBuilder.UpdateData(
                table: "GrantSectionsDetails",
                keyColumn: "Id",
                keyValue: 6,
                column: "SectionId",
                value: null);

            migrationBuilder.UpdateData(
                table: "GrantSectionsDetails",
                keyColumn: "Id",
                keyValue: 7,
                column: "SectionId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_GrantSectionsDetails_SectionId",
                table: "GrantSectionsDetails",
                column: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_GrantSectionsDetails_GrantRejectionShortfallSection_Section~",
                table: "GrantSectionsDetails",
                column: "SectionId",
                principalTable: "GrantRejectionShortfallSection",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GrantSectionsDetails_GrantRejectionShortfallSection_Section~",
                table: "GrantSectionsDetails");

            migrationBuilder.DropIndex(
                name: "IX_GrantSectionsDetails_SectionId",
                table: "GrantSectionsDetails");

            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "GrantSectionsDetails");


            migrationBuilder.CreateIndex(
                name: "IX_GrantRejectionShortfallSection_SectionId",
                table: "GrantRejectionShortfallSection",
                column: "SectionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GrantRejectionShortfallSection_GrantSectionsDetails_Section~",
                table: "GrantRejectionShortfallSection",
                column: "SectionId",
                principalTable: "GrantSectionsDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
