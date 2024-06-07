using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class RemoveduserForeignKeysandAddedDistrictTable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DivisionDetails_AspNetUsers_CreatedBy",
                table: "DivisionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_DivisionDetails_AspNetUsers_UpdatedBy",
                table: "DivisionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_SubDivisionDetails_AspNetUsers_CreatedBy",
                table: "SubDivisionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_SubDivisionDetails_AspNetUsers_UpdatedBy",
                table: "SubDivisionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TehsilBlockDetails_AspNetUsers_CreatedBy",
                table: "TehsilBlockDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TehsilBlockDetails_AspNetUsers_UpdatedBy",
                table: "TehsilBlockDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_VillageDetails_AspNetUsers_CreatedBy",
                table: "VillageDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_VillageDetails_AspNetUsers_UpdatedBy",
                table: "VillageDetails");

            migrationBuilder.DropIndex(
                name: "IX_VillageDetails_CreatedBy",
                table: "VillageDetails");

            migrationBuilder.DropIndex(
                name: "IX_VillageDetails_UpdatedBy",
                table: "VillageDetails");

            migrationBuilder.DropIndex(
                name: "IX_TehsilBlockDetails_CreatedBy",
                table: "TehsilBlockDetails");

            migrationBuilder.DropIndex(
                name: "IX_TehsilBlockDetails_UpdatedBy",
                table: "TehsilBlockDetails");

            migrationBuilder.DropIndex(
                name: "IX_SubDivisionDetails_CreatedBy",
                table: "SubDivisionDetails");

            migrationBuilder.DropIndex(
                name: "IX_SubDivisionDetails_UpdatedBy",
                table: "SubDivisionDetails");

            migrationBuilder.DropIndex(
                name: "IX_DivisionDetails_CreatedBy",
                table: "DivisionDetails");

            migrationBuilder.DropIndex(
                name: "IX_DivisionDetails_UpdatedBy",
                table: "DivisionDetails");

            migrationBuilder.AddColumn<int>(
                name: "DistrictId",
                table: "DivisionDetails",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "DistrictDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LGDID = table.Column<int>(name: "LGD_ID", type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistrictDetails", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "DistrictDetails",
                columns: new[] { "Id", "CreatedOn", "LGD_ID", "Name", "UpdatedOn" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 27, "Amritsar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 605, "Barnala", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 28, "Bathinda", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 29, "Faridkot", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 30, "Fatehgarh Sahib", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 651, "Fazilka", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 31, "Ferozepur", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 32, "Gurdaspur", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 33, "Hoshiarpur", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 34, "Jalandhar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 35, "Kapurthala", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 36, "Ludhiana", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 737, "Malerkotla", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 37, "Mansa", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 15, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 38, "Moga", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 16, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 662, "Pathankot", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 17, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 41, "Patiala", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 18, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 42, "Rupnagar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 19, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 608, "S.A.S Nagar Mohali", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 20, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 43, "Sangrur", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 21, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 40, "ShahidBhagat Singh Nagar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 22, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 39, "Sri Muktsar Sahib", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 23, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 609, "Tarn Taran", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DivisionDetails_DistrictId",
                table: "DivisionDetails",
                column: "DistrictId");

            migrationBuilder.AddForeignKey(
                name: "FK_DivisionDetails_DistrictDetails_DistrictId",
                table: "DivisionDetails",
                column: "DistrictId",
                principalTable: "DistrictDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DivisionDetails_DistrictDetails_DistrictId",
                table: "DivisionDetails");

            migrationBuilder.DropTable(
                name: "DistrictDetails");

            migrationBuilder.DropIndex(
                name: "IX_DivisionDetails_DistrictId",
                table: "DivisionDetails");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "DivisionDetails");

            migrationBuilder.CreateIndex(
                name: "IX_VillageDetails_CreatedBy",
                table: "VillageDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_VillageDetails_UpdatedBy",
                table: "VillageDetails",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TehsilBlockDetails_CreatedBy",
                table: "TehsilBlockDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TehsilBlockDetails_UpdatedBy",
                table: "TehsilBlockDetails",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SubDivisionDetails_CreatedBy",
                table: "SubDivisionDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SubDivisionDetails_UpdatedBy",
                table: "SubDivisionDetails",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionDetails_CreatedBy",
                table: "DivisionDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionDetails_UpdatedBy",
                table: "DivisionDetails",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_DivisionDetails_AspNetUsers_CreatedBy",
                table: "DivisionDetails",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DivisionDetails_AspNetUsers_UpdatedBy",
                table: "DivisionDetails",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubDivisionDetails_AspNetUsers_CreatedBy",
                table: "SubDivisionDetails",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubDivisionDetails_AspNetUsers_UpdatedBy",
                table: "SubDivisionDetails",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TehsilBlockDetails_AspNetUsers_CreatedBy",
                table: "TehsilBlockDetails",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TehsilBlockDetails_AspNetUsers_UpdatedBy",
                table: "TehsilBlockDetails",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VillageDetails_AspNetUsers_CreatedBy",
                table: "VillageDetails",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VillageDetails_AspNetUsers_UpdatedBy",
                table: "VillageDetails",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
