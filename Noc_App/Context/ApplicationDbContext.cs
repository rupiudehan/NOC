using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Noc_App.Models;
using Noc_App.Models.ViewModel;

namespace Noc_App.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<DistrictDetails> DistrictDetails { get; set; }
        public DbSet<DivisionDetails> DivisionDetails { get; set; }
        public DbSet<SubDivisionDetails> SubDivisionDetails { get; set; }
        public DbSet<TehsilBlockDetails> TehsilBlockDetails { get; set; }
        //public DbSet<VillageDetails> VillageDetails { get; set; }
        public DbSet<OwnerTypeDetails> OwnerTypeDetails { get; set; }
        public DbSet<OwnerDetails> OwnerDetails { get; set; }
        public DbSet<NocPermissionTypeDetails> NocPermissionTypeDetails { get; set; }
        public DbSet<NocTypeDetails> NocTypeDetails { get; set; }
        public DbSet<ProjectTypeDetails> ProjectTypeDetails { get; set; }
        public DbSet<SiteAreaUnitDetails> SiteAreaUnitDetails { get; set; }
        public DbSet<GrantApprovalMaster> GrantApprovalMaster { get; set; }
        public DbSet<GrantDetails> GrantDetails { get; set; }
        public DbSet<GrantKhasraDetails> GrantKhasraDetails { get; set; }
        public DbSet<GrantPaymentDetails> GrantPaymentDetails { get; set; }
        public DbSet<GrantApprovalDetail> GrantApprovalDetails { get; set; }
        public DbSet<GrantApprovalProcessDocumentsDetails> GrantApprovalProcessDocumentsDetails { get; set; }
        public DbSet<SiteUnitMaster> SiteUnitMaster { get; set; }
        public DbSet<ChallanDetails> ChallanDetails { get; set; }
        public DbSet<UserRoleDetails> UserRoleDetails { get; set; }
        public DbSet<DaysCheckMaster> DaysCheckMaster { get; set; }
        public DbSet<RecommendationDetail> RecommendationDetail { get; set; }
        public DbSet<GrantSectionsDetails> GrantSectionsDetails { get; set; }
        public DbSet<GrantRejectionShortfallSection> GrantRejectionShortfallSection { get; set; }
        public DbSet<PlanSanctionAuthorityMaster> PlanSanctionAuthorityMaster { get; set; }
        public DbSet<DrainWidthTypeDetails> DrainWidthTypeDetails { get; set; }
        public DbSet<GrantFileTransferDetails> GrantFileTransferDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GrantUnprocessedAppDetails>().HasNoKey();
            modelBuilder.Entity<DashboardPendencyAll>().HasNoKey();
            modelBuilder.Entity<DashboardPendencyViewModel>().HasNoKey();
            modelBuilder.Entity<ReportApplicationCountViewModel>().HasNoKey();


            modelBuilder.Entity<DrainWidthTypeDetails>().HasData(
                new DrainWidthTypeDetails { Id=1,Name="As Per Notification",Code="N"},
                new DrainWidthTypeDetails { Id=2,Name="As Per Calculation",Code="C"}
                );
            modelBuilder.Entity<PlanSanctionAuthorityMaster>().HasData(
                new PlanSanctionAuthorityMaster { Id = 1, Name = "Country", Code = "C" },
                new PlanSanctionAuthorityMaster { Id = 2, Name = "Local", Code = "L" }
                );

            modelBuilder.Entity<DistrictDetails>().HasData(
               new DistrictDetails { Id = 1, Name = "Amritsar", LGD_ID = 27 },
               new DistrictDetails { Id = 2, Name = "Barnala", LGD_ID = 605 },
               new DistrictDetails { Id = 3, Name = "Bathinda", LGD_ID = 28 },
               new DistrictDetails { Id = 4, Name = "Faridkot", LGD_ID = 29 },
               new DistrictDetails { Id = 5, Name = "Fatehgarh Sahib", LGD_ID = 30 },
               new DistrictDetails { Id = 6, Name = "Fazilka", LGD_ID = 651 },
               new DistrictDetails { Id = 7, Name = "Ferozepur", LGD_ID = 31 },
               new DistrictDetails { Id = 8, Name = "Gurdaspur", LGD_ID = 32 },
               new DistrictDetails { Id = 9, Name = "Hoshiarpur", LGD_ID = 33 },
               new DistrictDetails { Id = 10, Name = "Jalandhar", LGD_ID = 34 },
               new DistrictDetails { Id = 11, Name = "Kapurthala", LGD_ID = 35 },
               new DistrictDetails { Id = 12, Name = "Ludhiana", LGD_ID = 36 },
               new DistrictDetails { Id = 13, Name = "Malerkotla", LGD_ID = 737 },
               new DistrictDetails { Id = 14, Name = "Mansa", LGD_ID = 37 },
               new DistrictDetails { Id = 15, Name = "Moga", LGD_ID = 38 },
               new DistrictDetails { Id = 16, Name = "Pathankot", LGD_ID = 662 },
               new DistrictDetails { Id = 17, Name = "Patiala", LGD_ID = 41 },
               new DistrictDetails { Id = 18, Name = "Rupnagar", LGD_ID = 42 },
               new DistrictDetails { Id = 19, Name = "S.A.S Nagar Mohali", LGD_ID = 608 },
               new DistrictDetails { Id = 20, Name = "Sangrur", LGD_ID = 43 },
               new DistrictDetails { Id = 21, Name = "ShahidBhagat Singh Nagar", LGD_ID = 40 },
               new DistrictDetails { Id = 22, Name = "Sri Muktsar Sahib", LGD_ID = 39 },
               new DistrictDetails { Id = 23, Name = "Tarn Taran", LGD_ID = 609 }
               );

            modelBuilder.Entity<GrantSectionsDetails>().HasData( 
                new GrantSectionsDetails {Id=1,SectionCode="P",SectionName="Project" }, 
                new GrantSectionsDetails {Id=2,SectionCode="AD",SectionName="Address" },
                new GrantSectionsDetails { Id = 3, SectionCode = "KH", SectionName = "Khasra" },
                new GrantSectionsDetails { Id = 4, SectionCode = "K", SectionName = "KML" },
                new GrantSectionsDetails { Id = 5, SectionCode = "AP", SectionName = "Applicant" },
                new GrantSectionsDetails { Id = 6, SectionCode = "OW", SectionName = "Owner" },
                new GrantSectionsDetails { Id = 7, SectionCode = "PM", SectionName = "Permission" }
                );

            modelBuilder.Entity<RecommendationDetail>().HasData(
                new RecommendationDetail{ Id = 1, Name = "For Approval", Code = "A" },
                new RecommendationDetail { Id = 2, Name = "For Rejection", Code = "R" },
                new RecommendationDetail { Id = 3, Name = "Nothing", Code = "NA" }
                );

            modelBuilder.Entity<DaysCheckMaster>().HasData(
                new DaysCheckMaster { Id = 1, IsRelatedToForward=1,IsRelatedToIssue=0,CheckFor = "Executive Engineer", Code = "EEF", NoOfDays = 1,UserRoleID=7 },
                new DaysCheckMaster { Id = 9, IsRelatedToForward = 0, IsRelatedToIssue = 1, CheckFor = "Executive Engineer", Code = "EES", NoOfDays = 2,UserRoleID=7 },
                new DaysCheckMaster { Id = 2, IsRelatedToForward=1,IsRelatedToIssue=0,CheckFor = "Chief Engineer", Code = "CEHQF", NoOfDays =3,UserRoleID=10 },
                //new DaysCheckMaster { Id = 2, IsRelatedToForward=0, IsRelatedToIssue = 1, CheckFor = "Chief Engineer", Code = "CEHQS", NoOfDays = 1 },
                new DaysCheckMaster { Id = 3, IsRelatedToForward=1,IsRelatedToIssue=0,CheckFor = "Junior Engineer", Code = "JE", NoOfDays = 2,UserRoleID=60 },
                new DaysCheckMaster { Id = 4, IsRelatedToForward=1,IsRelatedToIssue=0,CheckFor = "Sub Divisional Officer", Code = "SDO", NoOfDays = 2,UserRoleID=67 },
                new DaysCheckMaster { Id = 5, IsRelatedToForward=1,IsRelatedToIssue=0,CheckFor = "XEN HO Drainage", Code = "EEHQ", NoOfDays = 3,UserRoleID=128 },
                new DaysCheckMaster { Id = 6, IsRelatedToForward=1,IsRelatedToIssue=0,CheckFor = "XEN/DWS", Code = "D", NoOfDays = 0.5,UserRoleID=83 },
                new DaysCheckMaster { Id = 7, IsRelatedToForward=1,IsRelatedToIssue=0,CheckFor = "Principal Secretary", Code = "PS", NoOfDays = 1,UserRoleID=6 },
                new DaysCheckMaster { Id = 8, IsRelatedToForward=1,IsRelatedToIssue=0,CheckFor = "Superintending Engineer", Code = "CO", NoOfDays = 2,UserRoleID=8 },
                new DaysCheckMaster { Id = 10, IsRelatedToForward = 1, IsRelatedToIssue = 0, CheckFor = "ADE/DWS", Code = "ADE", NoOfDays = 1, UserRoleID = 90 },
                new DaysCheckMaster { Id = 11, IsRelatedToForward = 1, IsRelatedToIssue = 0, CheckFor = "Director Drainage", Code = "DD", NoOfDays = 0.5, UserRoleID = 35 },
                new DaysCheckMaster { Id = 12, IsRelatedToForward = 0, IsRelatedToIssue = 0, CheckFor = "Shortfall", Code = "SF", NoOfDays = 7, UserRoleID = 7 }
                );

            modelBuilder.Entity<UserRoleDetails>().HasData(
                new UserRoleDetails { Id=7,RoleName= "Executive Engineer", AppRoleName= "EXECUTIVE ENGINEER", RoleLevel=7 },
                new UserRoleDetails { Id = 10, RoleName = "Chief Engineer",AppRoleName= "CHIEF ENGINEER HQ", RoleLevel = 3 },
                new UserRoleDetails { Id =60, RoleName = "Junior Engineer",AppRoleName= "JUNIOR ENGINEER", RoleLevel = 9 },
                new UserRoleDetails { Id = 67, RoleName = "Sub Divisional Officer",AppRoleName= "SUB DIVISIONAL OFFICER", RoleLevel = 8 },
                new UserRoleDetails { Id = 128, RoleName = "XEN HO Drainage",AppRoleName= "EXECUTIVE ENGINEER HQ", RoleLevel = 4 },
                new UserRoleDetails { Id = 83, RoleName = "XEN/DWS",AppRoleName="DWS", RoleLevel = 5 },
                new UserRoleDetails { Id = 6, RoleName = "Principal Secretary", AppRoleName = "PRINCIPAL SECRETARY", RoleLevel = 2 },
                new UserRoleDetails { Id = 8, RoleName = "Superintending Engineer", AppRoleName = "CIRCLE OFFICER", RoleLevel = 6 },
                new UserRoleDetails { Id = 1, RoleName = "Administrator", AppRoleName = "Administrator", RoleLevel = 1 }
                );

            modelBuilder.Entity<SiteUnitMaster>().HasData(
                new SiteUnitMaster { Id = 1,SiteAreaUnitId=1, UnitName = "Biswa",UnitCode="M",UnitValue= 1, Timesof=1, DivideBy= 32.27 },
                new SiteUnitMaster { Id = 2, SiteAreaUnitId = 1, UnitName = "Bigha", UnitCode = "K", UnitValue = 1, Timesof = 1, DivideBy = 1.673 },
                new SiteUnitMaster { Id = 3, SiteAreaUnitId = 1, UnitName = "Biswansi", UnitCode = "S", UnitValue = 1, Timesof = 1, DivideBy = 32.31 },
                new SiteUnitMaster { Id = 4, SiteAreaUnitId = 3, UnitName = "Biswa", UnitCode = "M", UnitValue = 1, Timesof = 3, DivideBy = 32.27 },
                new SiteUnitMaster { Id = 5, SiteAreaUnitId = 3, UnitName = "Bigha", UnitCode = "K", UnitValue = 1, Timesof = 3, DivideBy = 1.673 },
                new SiteUnitMaster { Id = 6, SiteAreaUnitId = 3, UnitName = "Biswansi", UnitCode = "S", UnitValue = 1, Timesof = 3, DivideBy = 32.31 },
                new SiteUnitMaster { Id = 7, SiteAreaUnitId = 2, UnitName = "Kanal", UnitCode = "K", UnitValue = 1, Timesof = 1, DivideBy = 8 },
                new SiteUnitMaster { Id = 8, SiteAreaUnitId = 2, UnitName = "Marla", UnitCode = "M", UnitValue = 1, Timesof = 1, DivideBy = 160 },
                new SiteUnitMaster { Id = 9, SiteAreaUnitId = 2, UnitName = "Sarsai", UnitCode = "S", UnitValue = 1, Timesof = 1, DivideBy = 1440 }
                );

            modelBuilder.Entity<SiteAreaUnitDetails>().HasData(
                new SiteAreaUnitDetails { Id = 1, Name = "Bigha/Biswa/Biswansi - Type-I(Kachcha)" },
                new SiteAreaUnitDetails { Id = 2, Name = "Kanal/Marla/Sarsai" },
                new SiteAreaUnitDetails { Id =3, Name = "Bigha/Biswa/Biswansi - Type-II(Pucca)" }
                );
            modelBuilder.Entity<ProjectTypeDetails>().HasData(
                new ProjectTypeDetails { Id = 1,Name = "Residentials" },
                new ProjectTypeDetails { Id = 2,Name = "Industrial" },
                new ProjectTypeDetails { Id = 3,Name = "Commercial" },
                new ProjectTypeDetails { Id = 4, Name = "Any Other" }
                );
            modelBuilder.Entity<OwnerTypeDetails>().HasData(
                new OwnerTypeDetails { Id = 1,Name = "Owner" },
                new OwnerTypeDetails { Id = 2,Name = "Partners" },
                new OwnerTypeDetails { Id = 3,Name = "Chief Executive" },
                new OwnerTypeDetails { Id = 4,Name = "Full Time Directors" }
                );
            modelBuilder.Entity<NocPermissionTypeDetails>().HasData(
                new NocPermissionTypeDetails {Id = 1,Name = "Residential" },
                new NocPermissionTypeDetails {Id = 2,Name = "Industrial" },
                new NocPermissionTypeDetails { Id = 3, Name = "Commercial" }
                );
            modelBuilder.Entity<NocTypeDetails>().HasData(
                new NocTypeDetails {Id = 1, Name = "New" },
                new NocTypeDetails { Id = 2, Name = "Extension of Existing Project" }
                );
            modelBuilder.Entity<GrantApprovalMaster>().HasData(
                new GrantApprovalMaster { Id = 1, Name = "Pending", Code = "P" },
                new GrantApprovalMaster { Id = 2, Name = "Reject", Code = "R" },
                new GrantApprovalMaster { Id = 3, Name = "Forward", Code = "F" },
                new GrantApprovalMaster { Id = 4, Name = "Approved", Code = "A" },
                new GrantApprovalMaster { Id = 5, Name = "ShortFall", Code = "SF" }
                );

            base.OnModelCreating(modelBuilder);
        }

    }
}
