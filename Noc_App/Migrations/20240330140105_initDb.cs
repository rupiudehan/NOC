using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class initDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
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
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GrantApprovalMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrantApprovalMaster", x => x.Id);
                });

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
                name: "SiteAreaUnitDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteAreaUnitDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
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
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
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
                name: "DivisionDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DivisionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DivisionDetails_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DivisionDetails_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SubDivisionDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    DivisionId = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubDivisionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubDivisionDetails_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubDivisionDetails_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubDivisionDetails_DivisionDetails_DivisionId",
                        column: x => x.DivisionId,
                        principalTable: "DivisionDetails",
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
                name: "TehsilBlockDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    SubDivisionId = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TehsilBlockDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TehsilBlockDetails_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TehsilBlockDetails_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TehsilBlockDetails_SubDivisionDetails_SubDivisionId",
                        column: x => x.SubDivisionId,
                        principalTable: "SubDivisionDetails",
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
                name: "VillageDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    TehsilBlockId = table.Column<int>(type: "integer", nullable: false),
                    PinCode = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VillageDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VillageDetails_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VillageDetails_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VillageDetails_TehsilBlockDetails_TehsilBlockId",
                        column: x => x.TehsilBlockId,
                        principalTable: "TehsilBlockDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GrantDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    SiteAreaUnitId = table.Column<int>(type: "integer", nullable: false),
                    ProjectTypeId = table.Column<int>(type: "integer", nullable: false),
                    OtherProjectTypeDetail = table.Column<string>(type: "text", nullable: true),
                    Hadbast = table.Column<string>(type: "text", nullable: true),
                    PlotNo = table.Column<string>(type: "text", nullable: true),
                    VillageID = table.Column<int>(type: "integer", nullable: false),
                    AddressProofPhotoPath = table.Column<string>(type: "text", nullable: true),
                    KMLFilePath = table.Column<string>(type: "text", nullable: true),
                    KMLLinkName = table.Column<string>(type: "text", nullable: true),
                    ApplicantName = table.Column<string>(type: "text", nullable: true),
                    ApplicantEmailID = table.Column<string>(type: "text", nullable: true),
                    IDProofPhotoPath = table.Column<string>(type: "text", nullable: true),
                    AuthorizationLetterPhotoPath = table.Column<string>(type: "text", nullable: true),
                    NocPermissionTypeID = table.Column<int>(type: "integer", nullable: false),
                    NocTypeId = table.Column<int>(type: "integer", nullable: false),
                    IsExtension = table.Column<bool>(type: "boolean", nullable: false),
                    NocNumber = table.Column<string>(type: "text", nullable: true),
                    PreviousDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    ApplicationID = table.Column<string>(type: "text", nullable: true),
                    IsPending = table.Column<bool>(type: "boolean", nullable: false),
                    IsCancelled = table.Column<bool>(type: "boolean", nullable: false),
                    IsRejected = table.Column<bool>(type: "boolean", nullable: false),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: false),
                    IsForwarded = table.Column<bool>(type: "boolean", nullable: false),
                    ForwardLevel = table.Column<int>(type: "integer", nullable: false),
                    IsSentBack = table.Column<bool>(type: "boolean", nullable: false),
                    SentBackLevel = table.Column<int>(type: "integer", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrantDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrantDetails_NocPermissionTypeDetails_NocPermissionTypeID",
                        column: x => x.NocPermissionTypeID,
                        principalTable: "NocPermissionTypeDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GrantDetails_NocTypeDetails_NocTypeId",
                        column: x => x.NocTypeId,
                        principalTable: "NocTypeDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GrantDetails_ProjectTypeDetails_ProjectTypeId",
                        column: x => x.ProjectTypeId,
                        principalTable: "ProjectTypeDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GrantDetails_SiteAreaUnitDetails_SiteAreaUnitId",
                        column: x => x.SiteAreaUnitId,
                        principalTable: "SiteAreaUnitDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GrantDetails_VillageDetails_VillageID",
                        column: x => x.VillageID,
                        principalTable: "VillageDetails",
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

            migrationBuilder.CreateTable(
                name: "GrantKhasraDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KhasraNo = table.Column<string>(type: "text", nullable: true),
                    UnitId = table.Column<int>(type: "integer", nullable: false),
                    GrantID = table.Column<int>(type: "integer", nullable: false),
                    MarlaOrBiswansi = table.Column<double>(type: "double precision", nullable: false),
                    KanalOrBiswa = table.Column<double>(type: "double precision", nullable: false),
                    SarsaiOrBigha = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrantKhasraDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrantKhasraDetails_GrantDetails_GrantID",
                        column: x => x.GrantID,
                        principalTable: "GrantDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GrantKhasraDetails_SiteAreaUnitDetails_UnitId",
                        column: x => x.UnitId,
                        principalTable: "SiteAreaUnitDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OwnerDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerTypeId = table.Column<int>(type: "integer", nullable: false),
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_DivisionDetails_CreatedBy",
                table: "DivisionDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionDetails_UpdatedBy",
                table: "DivisionDetails",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GrantDetails_NocPermissionTypeID",
                table: "GrantDetails",
                column: "NocPermissionTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_GrantDetails_NocTypeId",
                table: "GrantDetails",
                column: "NocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GrantDetails_ProjectTypeId",
                table: "GrantDetails",
                column: "ProjectTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GrantDetails_SiteAreaUnitId",
                table: "GrantDetails",
                column: "SiteAreaUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_GrantDetails_VillageID",
                table: "GrantDetails",
                column: "VillageID");

            migrationBuilder.CreateIndex(
                name: "IX_GrantKhasraDetails_GrantID",
                table: "GrantKhasraDetails",
                column: "GrantID");

            migrationBuilder.CreateIndex(
                name: "IX_GrantKhasraDetails_UnitId",
                table: "GrantKhasraDetails",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerDetails_GrantId",
                table: "OwnerDetails",
                column: "GrantId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerDetails_OwnerTypeId",
                table: "OwnerDetails",
                column: "OwnerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SubDivisionDetails_CreatedBy",
                table: "SubDivisionDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SubDivisionDetails_DivisionId",
                table: "SubDivisionDetails",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubDivisionDetails_UpdatedBy",
                table: "SubDivisionDetails",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TehsilBlockDetails_CreatedBy",
                table: "TehsilBlockDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TehsilBlockDetails_SubDivisionId",
                table: "TehsilBlockDetails",
                column: "SubDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_TehsilBlockDetails_UpdatedBy",
                table: "TehsilBlockDetails",
                column: "UpdatedBy");

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

            migrationBuilder.CreateIndex(
                name: "IX_VillageDetails_CreatedBy",
                table: "VillageDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_VillageDetails_TehsilBlockId",
                table: "VillageDetails",
                column: "TehsilBlockId");

            migrationBuilder.CreateIndex(
                name: "IX_VillageDetails_UpdatedBy",
                table: "VillageDetails",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "GrantApprovalMaster");

            migrationBuilder.DropTable(
                name: "GrantKhasraDetails");

            migrationBuilder.DropTable(
                name: "OwnerDetails");

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
                name: "GrantDetails");

            migrationBuilder.DropTable(
                name: "OwnerTypeDetails");

            migrationBuilder.DropTable(
                name: "NocPermissionTypeDetails");

            migrationBuilder.DropTable(
                name: "NocTypeDetails");

            migrationBuilder.DropTable(
                name: "ProjectTypeDetails");

            migrationBuilder.DropTable(
                name: "SiteAreaUnitDetails");

            migrationBuilder.DropTable(
                name: "VillageDetails");

            migrationBuilder.DropTable(
                name: "TehsilBlockDetails");

            migrationBuilder.DropTable(
                name: "SubDivisionDetails");

            migrationBuilder.DropTable(
                name: "DivisionDetails");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
