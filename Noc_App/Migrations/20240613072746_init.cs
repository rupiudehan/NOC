using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NocApp.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "PlanSanctionAuthorityMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanSanctionAuthorityMaster", x => x.Id);
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
                name: "RecommendationDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecommendationDetail", x => x.Id);
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
                name: "UserRoleDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleName = table.Column<string>(type: "text", nullable: true),
                    AppRoleName = table.Column<string>(type: "text", nullable: true),
                    RoleLevel = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DivisionDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    DistrictId = table.Column<int>(type: "integer", nullable: false),
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
                        name: "FK_DivisionDetails_DistrictDetails_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "DistrictDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SiteUnitMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SiteAreaUnitId = table.Column<int>(type: "integer", nullable: false),
                    UnitName = table.Column<string>(type: "text", nullable: true),
                    UnitCode = table.Column<string>(type: "text", nullable: true),
                    UnitValue = table.Column<double>(type: "double precision", nullable: false),
                    Timesof = table.Column<double>(type: "double precision", nullable: false),
                    DivideBy = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteUnitMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiteUnitMaster_SiteAreaUnitDetails_SiteAreaUnitId",
                        column: x => x.SiteAreaUnitId,
                        principalTable: "SiteAreaUnitDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DaysCheckMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CheckFor = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    IsRelatedToForward = table.Column<int>(type: "integer", nullable: false),
                    IsRelatedToIssue = table.Column<int>(type: "integer", nullable: false),
                    NoOfDays = table.Column<int>(type: "integer", nullable: false),
                    UserRoleID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DaysCheckMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DaysCheckMaster_UserRoleDetails_UserRoleID",
                        column: x => x.UserRoleID,
                        principalTable: "UserRoleDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        name: "FK_SubDivisionDetails_DivisionDetails_DivisionId",
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
                    LGDID = table.Column<int>(name: "LGD_ID", type: "integer", nullable: false),
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
                        name: "FK_TehsilBlockDetails_SubDivisionDetails_SubDivisionId",
                        column: x => x.SubDivisionId,
                        principalTable: "SubDivisionDetails",
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
                    PlanSanctionAuthorityId = table.Column<int>(type: "integer", nullable: false),
                    LayoutPlanFilePath = table.Column<string>(type: "text", nullable: true),
                    FaradFilePath = table.Column<string>(type: "text", nullable: true),
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
                    ProcessLevel = table.Column<int>(type: "integer", nullable: false),
                    IsSentBack = table.Column<bool>(type: "boolean", nullable: false),
                    SentBackLevel = table.Column<int>(type: "integer", nullable: false),
                    IsShortFall = table.Column<bool>(type: "boolean", nullable: false),
                    ShortFallLevel = table.Column<int>(type: "integer", nullable: false),
                    ShortFallReportedById = table.Column<string>(type: "text", nullable: true),
                    ShortFallReportedByRole = table.Column<string>(type: "text", nullable: true),
                    ShortFallReportedByName = table.Column<string>(type: "text", nullable: true),
                    ShortFallReportedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsShortFallCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    ShortFallCompletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CertificateFilePath = table.Column<string>(type: "text", nullable: true),
                    UploadedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UploadedByRole = table.Column<string>(type: "text", nullable: true),
                    UploadedBy = table.Column<string>(type: "text", nullable: true)
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
                        name: "FK_GrantDetails_PlanSanctionAuthorityMaster_PlanSanctionAuthor~",
                        column: x => x.PlanSanctionAuthorityId,
                        principalTable: "PlanSanctionAuthorityMaster",
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
                name: "GrantApprovalDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GrantID = table.Column<int>(type: "integer", nullable: false),
                    ApprovalID = table.Column<int>(type: "integer", nullable: false),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    ProcessLevel = table.Column<int>(type: "integer", nullable: false),
                    ProcessedToUser = table.Column<string>(type: "text", nullable: true),
                    ProcessedToRole = table.Column<string>(type: "text", nullable: true),
                    ProcessedToName = table.Column<string>(type: "text", nullable: true),
                    ProcessedBy = table.Column<string>(type: "text", nullable: true),
                    ProcessedByRole = table.Column<string>(type: "text", nullable: true),
                    ProcessedByName = table.Column<string>(type: "text", nullable: true),
                    ProcessedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RecommendationID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrantApprovalDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrantApprovalDetails_GrantApprovalMaster_ApprovalID",
                        column: x => x.ApprovalID,
                        principalTable: "GrantApprovalMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GrantApprovalDetails_GrantDetails_GrantID",
                        column: x => x.GrantID,
                        principalTable: "GrantDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GrantApprovalDetails_RecommendationDetail_RecommendationID",
                        column: x => x.RecommendationID,
                        principalTable: "RecommendationDetail",
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
                    MarlaOrBiswa = table.Column<double>(type: "double precision", nullable: false),
                    KanalOrBigha = table.Column<double>(type: "double precision", nullable: false),
                    SarsaiOrBiswansi = table.Column<double>(type: "double precision", nullable: false)
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
                name: "GrantPaymentDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GrantID = table.Column<int>(type: "integer", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    deptRefNo = table.Column<string>(type: "text", nullable: true),
                    PaymentOrderId = table.Column<string>(type: "text", nullable: true),
                    PayerName = table.Column<string>(type: "text", nullable: true),
                    PayerEmail = table.Column<string>(type: "text", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "GrantApprovalProcessDocumentsDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GrantApprovalID = table.Column<long>(type: "bigint", nullable: false),
                    SiteConditionReportPath = table.Column<string>(type: "text", nullable: true),
                    CatchmentAreaAndFlowPath = table.Column<string>(type: "text", nullable: true),
                    DistanceFromCreekPath = table.Column<string>(type: "text", nullable: true),
                    GISOrDWSReportPath = table.Column<string>(type: "text", nullable: true),
                    IsKMLByApplicantValid = table.Column<bool>(type: "boolean", nullable: false),
                    CrossSectionOrCalculationSheetReportPath = table.Column<string>(type: "text", nullable: true),
                    DrainLSectionPath = table.Column<string>(type: "text", nullable: true),
                    ProcessedBy = table.Column<string>(type: "text", nullable: true),
                    ProcessedByRole = table.Column<string>(type: "text", nullable: true),
                    ProcessedByName = table.Column<string>(type: "text", nullable: true),
                    ProcessedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedByRole = table.Column<string>(type: "text", nullable: true),
                    UpdatedByName = table.Column<string>(type: "text", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrantApprovalProcessDocumentsDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrantApprovalProcessDocumentsDetails_GrantApprovalDetails_G~",
                        column: x => x.GrantApprovalID,
                        principalTable: "GrantApprovalDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GrantRejectionShortfallSection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GrantApprovalId = table.Column<long>(type: "bigint", nullable: false),
                    SectionId = table.Column<int>(type: "integer", nullable: false),
                    IsCompleted = table.Column<int>(type: "integer", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrantRejectionShortfallSection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrantRejectionShortfallSection_GrantApprovalDetails_GrantAp~",
                        column: x => x.GrantApprovalId,
                        principalTable: "GrantApprovalDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GrantSectionsDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SectionName = table.Column<string>(type: "text", nullable: true),
                    SectionCode = table.Column<string>(type: "text", nullable: true),
                    SectionId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrantSectionsDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrantSectionsDetails_GrantRejectionShortfallSection_Section~",
                        column: x => x.SectionId,
                        principalTable: "GrantRejectionShortfallSection",
                        principalColumn: "Id");
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

            migrationBuilder.InsertData(
                table: "GrantApprovalMaster",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { 1, "P", "Pending" },
                    { 2, "R", "Reject" },
                    { 3, "F", "Forward" },
                    { 4, "A", "Approved" },
                    { 5, "SF", "ShortFall" }
                });

            migrationBuilder.InsertData(
                table: "GrantSectionsDetails",
                columns: new[] { "Id", "SectionCode", "SectionId", "SectionName" },
                values: new object[,]
                {
                    { 1, "P", null, "Project" },
                    { 2, "AD", null, "Address" },
                    { 3, "KH", null, "Khasra" },
                    { 4, "K", null, "KML" },
                    { 5, "AP", null, "Applicant" },
                    { 6, "OW", null, "Owner" },
                    { 7, "PM", null, "Permission" }
                });

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
                table: "PlanSanctionAuthorityMaster",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { 1, "C", "Country" },
                    { 2, "L", "Local" }
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
                table: "RecommendationDetail",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { 1, "A", "For Approval" },
                    { 2, "R", "For Rejection" },
                    { 3, "NA", "Nothing" }
                });

            migrationBuilder.InsertData(
                table: "SiteAreaUnitDetails",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Bigha/Biswa/Biswansi - Type-I(Kachcha)" },
                    { 2, "Kanal/Marla/Sarsai" },
                    { 3, "Bigha/Biswa/Biswansi - Type-II(Pucca)" }
                });

            migrationBuilder.InsertData(
                table: "UserRoleDetails",
                columns: new[] { "Id", "AppRoleName", "RoleLevel", "RoleName" },
                values: new object[,]
                {
                    { 1, "Administrator", 1, "Administrator" },
                    { 6, "PRINCIPAL SECRETARY", 2, "Principal Secretary" },
                    { 7, "EXECUTIVE ENGINEER", 7, "Executive Engineer" },
                    { 8, "CIRCLE OFFICER", 6, "Superintending Engineer" },
                    { 10, "CHIEF ENGINEER DRAINAGE", 3, "Chief Engineer" },
                    { 60, "JUNIOR ENGINEER", 9, "Junior Engineer" },
                    { 67, "SUB DIVISIONAL OFFICER", 8, "Sub Divisional Officer" },
                    { 83, "DWS", 5, "XEN/DWS" },
                    { 128, "EXECUTIVE ENGINEER DRAINAGE", 4, "XEN HO Drainage" }
                });

            migrationBuilder.InsertData(
                table: "DaysCheckMaster",
                columns: new[] { "Id", "CheckFor", "Code", "IsRelatedToForward", "IsRelatedToIssue", "NoOfDays", "UserRoleID" },
                values: new object[,]
                {
                    { 1, "Executive Engineer", "EEF", 1, 0, 1, 7 },
                    { 2, "Chief Engineer", "CEHQF", 1, 0, 3, 10 },
                    { 3, "Junior Engineer", "JE", 1, 0, 2, 60 },
                    { 4, "Sub Divisional Officer", "SDO", 1, 0, 2, 67 },
                    { 5, "XEN HO Drainage", "EEHQ", 1, 0, 3, 128 },
                    { 6, "XEN/DWS", "D", 1, 0, 1, 83 },
                    { 7, "Principal Secretary", "PS", 1, 0, 1, 6 },
                    { 8, "Superintending Engineer", "CO", 1, 0, 2, 8 },
                    { 9, "Executive Engineer", "EES", 0, 1, 2, 7 }
                });

            migrationBuilder.InsertData(
                table: "SiteUnitMaster",
                columns: new[] { "Id", "DivideBy", "SiteAreaUnitId", "Timesof", "UnitCode", "UnitName", "UnitValue" },
                values: new object[,]
                {
                    { 1, 32.270000000000003, 1, 1.0, "M", "Biswa", 1.0 },
                    { 2, 1.673, 1, 1.0, "K", "Bigha", 1.0 },
                    { 3, 32.310000000000002, 1, 1.0, "S", "Biswansi", 1.0 },
                    { 4, 32.270000000000003, 3, 3.0, "M", "Biswa", 1.0 },
                    { 5, 1.673, 3, 3.0, "K", "Bigha", 1.0 },
                    { 6, 32.310000000000002, 3, 3.0, "S", "Biswansi", 1.0 },
                    { 7, 8.0, 2, 1.0, "K", "Kanal", 1.0 },
                    { 8, 160.0, 2, 1.0, "M", "Marla", 1.0 },
                    { 9, 1440.0, 2, 1.0, "S", "Sarsai", 1.0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DaysCheckMaster_UserRoleID",
                table: "DaysCheckMaster",
                column: "UserRoleID");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionDetails_DistrictId",
                table: "DivisionDetails",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_GrantApprovalDetails_ApprovalID",
                table: "GrantApprovalDetails",
                column: "ApprovalID");

            migrationBuilder.CreateIndex(
                name: "IX_GrantApprovalDetails_GrantID",
                table: "GrantApprovalDetails",
                column: "GrantID");

            migrationBuilder.CreateIndex(
                name: "IX_GrantApprovalDetails_RecommendationID",
                table: "GrantApprovalDetails",
                column: "RecommendationID");

            migrationBuilder.CreateIndex(
                name: "IX_GrantApprovalProcessDocumentsDetails_GrantApprovalID",
                table: "GrantApprovalProcessDocumentsDetails",
                column: "GrantApprovalID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GrantDetails_NocPermissionTypeID",
                table: "GrantDetails",
                column: "NocPermissionTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_GrantDetails_NocTypeId",
                table: "GrantDetails",
                column: "NocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GrantDetails_PlanSanctionAuthorityId",
                table: "GrantDetails",
                column: "PlanSanctionAuthorityId");

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
                name: "IX_GrantPaymentDetails_GrantID",
                table: "GrantPaymentDetails",
                column: "GrantID");

            migrationBuilder.CreateIndex(
                name: "IX_GrantRejectionShortfallSection_GrantApprovalId",
                table: "GrantRejectionShortfallSection",
                column: "GrantApprovalId");

            migrationBuilder.CreateIndex(
                name: "IX_GrantSectionsDetails_SectionId",
                table: "GrantSectionsDetails",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerDetails_GrantId",
                table: "OwnerDetails",
                column: "GrantId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerDetails_OwnerTypeId",
                table: "OwnerDetails",
                column: "OwnerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SiteUnitMaster_SiteAreaUnitId",
                table: "SiteUnitMaster",
                column: "SiteAreaUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_SubDivisionDetails_DivisionId",
                table: "SubDivisionDetails",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_TehsilBlockDetails_SubDivisionId",
                table: "TehsilBlockDetails",
                column: "SubDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_VillageDetails_TehsilBlockId",
                table: "VillageDetails",
                column: "TehsilBlockId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChallanDetails");

            migrationBuilder.DropTable(
                name: "DaysCheckMaster");

            migrationBuilder.DropTable(
                name: "GrantApprovalProcessDocumentsDetails");

            migrationBuilder.DropTable(
                name: "GrantKhasraDetails");

            migrationBuilder.DropTable(
                name: "GrantPaymentDetails");

            migrationBuilder.DropTable(
                name: "GrantSectionsDetails");

            migrationBuilder.DropTable(
                name: "OwnerDetails");

            migrationBuilder.DropTable(
                name: "SiteUnitMaster");

            migrationBuilder.DropTable(
                name: "UserRoleDetails");

            migrationBuilder.DropTable(
                name: "GrantRejectionShortfallSection");

            migrationBuilder.DropTable(
                name: "OwnerTypeDetails");

            migrationBuilder.DropTable(
                name: "GrantApprovalDetails");

            migrationBuilder.DropTable(
                name: "GrantApprovalMaster");

            migrationBuilder.DropTable(
                name: "GrantDetails");

            migrationBuilder.DropTable(
                name: "RecommendationDetail");

            migrationBuilder.DropTable(
                name: "NocPermissionTypeDetails");

            migrationBuilder.DropTable(
                name: "NocTypeDetails");

            migrationBuilder.DropTable(
                name: "PlanSanctionAuthorityMaster");

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
                name: "DistrictDetails");
        }
    }
}
