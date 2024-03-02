using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedGrantOwnersOwnerTypeNocPermisionNocTypeProjectType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NocPermissionTypeDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NocPermissionTypeDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NocTypeDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NocTypeDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OwnerTypeDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnerTypeDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTypeDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTypeDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GrantDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    SiteAreaOrSizeInFeet = table.Column<decimal>(type: "numeric", nullable: false),
                    SiteAreaOrSizeInInches = table.Column<decimal>(type: "numeric", nullable: false),
                    ProjectTypeId = table.Column<int>(type: "integer", nullable: true),
                    Khasra = table.Column<string>(type: "text", nullable: true),
                    Hadbast = table.Column<string>(type: "text", nullable: true),
                    PlotNo = table.Column<string>(type: "text", nullable: true),
                    VillageId = table.Column<int>(type: "integer", nullable: true),
                    AddressProofPhotoPath = table.Column<string>(type: "text", nullable: true),
                    Latitude = table.Column<decimal>(type: "numeric", nullable: false),
                    Longitute = table.Column<decimal>(type: "numeric", nullable: false),
                    ApplicantName = table.Column<string>(type: "text", nullable: false),
                    ApplicantEmailID = table.Column<string>(type: "text", nullable: false),
                    IDProofPhotoPath = table.Column<string>(type: "text", nullable: true),
                    AuthorizationLetterPhotoPath = table.Column<string>(type: "text", nullable: true),
                    NocPermissionTypeId = table.Column<int>(type: "integer", nullable: true),
                    NocTypeId = table.Column<int>(type: "integer", nullable: true),
                    IsExtension = table.Column<bool>(type: "boolean", nullable: false),
                    NocNumber = table.Column<string>(type: "text", nullable: true),
                    PreviousDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    ApplicationID = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrantDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrantDetails_NocPermissionTypeDetails_NocPermissionTypeId",
                        column: x => x.NocPermissionTypeId,
                        principalTable: "NocPermissionTypeDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GrantDetails_NocTypeDetails_NocTypeId",
                        column: x => x.NocTypeId,
                        principalTable: "NocTypeDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GrantDetails_ProjectTypeDetails_ProjectTypeId",
                        column: x => x.ProjectTypeId,
                        principalTable: "ProjectTypeDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GrantDetails_VillageDetails_VillageId",
                        column: x => x.VillageId,
                        principalTable: "VillageDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OwnerDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerTypeId = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    MobileNo = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    GrantId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnerDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OwnerDetails_GrantDetails_GrantId",
                        column: x => x.GrantId,
                        principalTable: "GrantDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OwnerDetails_OwnerTypeDetails_OwnerTypeId",
                        column: x => x.OwnerTypeId,
                        principalTable: "OwnerTypeDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GrantDetails_NocPermissionTypeId",
                table: "GrantDetails",
                column: "NocPermissionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GrantDetails_NocTypeId",
                table: "GrantDetails",
                column: "NocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GrantDetails_ProjectTypeId",
                table: "GrantDetails",
                column: "ProjectTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GrantDetails_VillageId",
                table: "GrantDetails",
                column: "VillageId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerDetails_GrantId",
                table: "OwnerDetails",
                column: "GrantId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerDetails_OwnerTypeId",
                table: "OwnerDetails",
                column: "OwnerTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OwnerDetails");

            migrationBuilder.DropTable(
                name: "GrantDetails");

            migrationBuilder.DropTable(
                name: "OwnerTypeDetails");

            migrationBuilder.DropTable(
                name: "NocPermissionTypeDetails");

            migrationBuilder.DropTable(
                name: "NocTypeDetails");

            migrationBuilder.DropTable(
                name: "ProjectTypeDetails");
        }
    }
}
