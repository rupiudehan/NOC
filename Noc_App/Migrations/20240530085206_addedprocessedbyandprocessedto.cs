using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class addedprocessedbyandprocessedto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "UserDivision");

            migrationBuilder.DropTable(
                name: "UserSubdivision");

            migrationBuilder.DropTable(
                name: "UserTehsil");

            migrationBuilder.DropTable(
                name: "UserVillage");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "ProcessedByName",
                table: "GrantApprovalDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProcessedToName",
                table: "GrantApprovalDetails",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "LGD_ID",
                value: 27);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 2,
                column: "LGD_ID",
                value: 605);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 3,
                column: "LGD_ID",
                value: 28);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 4,
                column: "LGD_ID",
                value: 29);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 5,
                column: "LGD_ID",
                value: 30);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 6,
                column: "LGD_ID",
                value: 651);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 7,
                column: "LGD_ID",
                value: 31);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 8,
                column: "LGD_ID",
                value: 32);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 9,
                column: "LGD_ID",
                value: 33);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 10,
                column: "LGD_ID",
                value: 34);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 11,
                column: "LGD_ID",
                value: 35);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 12,
                column: "LGD_ID",
                value: 36);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 13,
                column: "LGD_ID",
                value: 737);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 14,
                column: "LGD_ID",
                value: 37);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 15,
                column: "LGD_ID",
                value: 38);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 16,
                column: "LGD_ID",
                value: 662);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 17,
                column: "LGD_ID",
                value: 41);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 18,
                column: "LGD_ID",
                value: 42);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 19,
                column: "LGD_ID",
                value: 608);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 20,
                column: "LGD_ID",
                value: 43);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 21,
                column: "LGD_ID",
                value: 40);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 22,
                column: "LGD_ID",
                value: 39);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 23,
                column: "LGD_ID",
                value: 609);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessedByName",
                table: "GrantApprovalDetails");

            migrationBuilder.DropColumn(
                name: "ProcessedToName",
                table: "GrantApprovalDetails");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDivision",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    DivisionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDivision", x => new { x.UserId, x.DivisionId });
                    table.ForeignKey(
                        name: "FK_UserDivision_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDivision_DivisionDetails_DivisionId",
                        column: x => x.DivisionId,
                        principalTable: "DivisionDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSubdivision",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    SubdivisionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSubdivision", x => new { x.UserId, x.SubdivisionId });
                    table.ForeignKey(
                        name: "FK_UserSubdivision_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSubdivision_SubDivisionDetails_SubdivisionId",
                        column: x => x.SubdivisionId,
                        principalTable: "SubDivisionDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTehsil",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    TehsilId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTehsil", x => new { x.UserId, x.TehsilId });
                    table.ForeignKey(
                        name: "FK_UserTehsil_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTehsil_TehsilBlockDetails_TehsilId",
                        column: x => x.TehsilId,
                        principalTable: "TehsilBlockDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserVillage",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    VillageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVillage", x => new { x.UserId, x.VillageId });
                    table.ForeignKey(
                        name: "FK_UserVillage_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserVillage_VillageDetails_VillageId",
                        column: x => x.VillageId,
                        principalTable: "VillageDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 2,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 3,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 4,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 5,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 6,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 7,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 8,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 9,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 10,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 11,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 12,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 13,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 14,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 15,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 16,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 17,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 18,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 19,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 20,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 21,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 22,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "DistrictDetails",
                keyColumn: "Id",
                keyValue: 23,
                column: "LGD_ID",
                value: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDivision_DivisionId",
                table: "UserDivision",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubdivision_SubdivisionId",
                table: "UserSubdivision",
                column: "SubdivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTehsil_TehsilId",
                table: "UserTehsil",
                column: "TehsilId");

            migrationBuilder.CreateIndex(
                name: "IX_UserVillage_VillageId",
                table: "UserVillage",
                column: "VillageId");
        }
    }
}
