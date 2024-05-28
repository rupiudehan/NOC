using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Noc_App.Models;
using Noc_App.Models.ViewModel;

namespace Noc_App.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //public DbSet<Employee> Employee { get; set; }
        public DbSet<DivisionDetails> DivisionDetails { get; set; }
        public DbSet<SubDivisionDetails> SubDivisionDetails { get; set; }
        public DbSet<TehsilBlockDetails> TehsilBlockDetails { get; set; }
        public DbSet<VillageDetails> VillageDetails { get; set; }
        public DbSet<UserDivision> UserDivision { get; set; }
        public DbSet<UserSubdivision> UserSubdivision { get; set; }
        public DbSet<UserTehsil> UserTehsil { get; set; }
        public DbSet<UserVillage> UserVillage { get; set; }
        //public DbSet<DrainCoordinatesDetails> DrainCoordinatesDetails { get; set; }
        //public DbSet<DrainDetails> DrainDetails { get; set; }
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GrantUnprocessedAppDetails>().HasNoKey();
            modelBuilder.Entity<DashboardPendencyAll>().HasNoKey();
            modelBuilder.Entity<DashboardPendencyViewModel>().HasNoKey();
            modelBuilder.Entity<UserDivision>()
            .HasKey(ud => new { ud.UserId, ud.DivisionId });

            modelBuilder.Entity<UserDivision>()
                .HasOne(ud => ud.User)
                .WithMany(u => u.UserDivisions)
                .HasForeignKey(ud => ud.UserId);

            modelBuilder.Entity<UserDivision>()
                .HasOne(ud => ud.Division)
                .WithMany(d => d.UserDivisions)
                .HasForeignKey(ud => ud.DivisionId);

            modelBuilder.Entity<UserSubdivision>()
            .HasKey(ud => new { ud.UserId, ud.SubdivisionId });

            modelBuilder.Entity<UserSubdivision>()
                .HasOne(ud => ud.User)
                .WithMany(u => u.UserSubdivisions)
                .HasForeignKey(ud => ud.UserId);

            modelBuilder.Entity<UserSubdivision>()
                .HasOne(ud => ud.Subdivision)
                .WithMany(d => d.UserSubdivisions)
                .HasForeignKey(ud => ud.SubdivisionId);

            modelBuilder.Entity<UserTehsil>()
            .HasKey(ud => new { ud.UserId, ud.TehsilId });

            modelBuilder.Entity<UserTehsil>()
                .HasOne(ud => ud.User)
                .WithMany(u => u.UserTehsils)
                .HasForeignKey(ud => ud.UserId);

            modelBuilder.Entity<UserTehsil>()
                .HasOne(ud => ud.Tehsil)
                .WithMany(d => d.UserTehsils)
                .HasForeignKey(ud => ud.TehsilId);

            modelBuilder.Entity<UserVillage>()
            .HasKey(ud => new { ud.UserId, ud.VillageId });

            modelBuilder.Entity<UserVillage>()
                .HasOne(ud => ud.User)
                .WithMany(u => u.UserVillages)
                .HasForeignKey(ud => ud.UserId);

            modelBuilder.Entity<UserVillage>()
                .HasOne(ud => ud.Village)
                .WithMany(d => d.UserVillages)
                .HasForeignKey(ud => ud.VillageId);

            modelBuilder.Entity<VillageDetails>()
                .HasOne(v => v.User)
                .WithMany(u => u.Villages)
                .HasForeignKey(v => v.CreatedBy);
            modelBuilder.Entity<VillageDetails>().Ignore(d => d.User2);

            modelBuilder.Entity<VillageDetails>()
                .HasOne(v => v.User2)
                .WithMany(u => u.Villages2)
                .HasForeignKey(v => v.UpdatedBy);


            modelBuilder.Entity<TehsilBlockDetails>()
                .HasOne(c => c.User)
                .WithMany(u => u.TehsilBlocks)
                .HasForeignKey(c => c.CreatedBy);
            modelBuilder.Entity<TehsilBlockDetails>().Ignore(d => d.User2);
            modelBuilder.Entity<TehsilBlockDetails>()
                .HasOne(c => c.User2)
                .WithMany(u => u.TehsilBlocks2)
                .HasForeignKey(c => c.UpdatedBy);

            modelBuilder.Entity<SubDivisionDetails>()
                .HasOne(s => s.User)
                .WithMany(u => u.SubDivisions)
                .HasForeignKey(s => s.CreatedBy);
            modelBuilder.Entity<SubDivisionDetails>().Ignore(d => d.User2);
            modelBuilder.Entity<SubDivisionDetails>()
                .HasOne(s => s.User2)
                .WithMany(u => u.SubDivisions2)
                .HasForeignKey(s => s.UpdatedBy);

            modelBuilder.Entity<DivisionDetails>()
                .HasOne(s => s.User)
                .WithMany(u => u.Divisions)
                .HasForeignKey(s => s.CreatedBy);

            modelBuilder.Entity<DivisionDetails>().Ignore(d => d.User2);

            modelBuilder.Entity<DivisionDetails>()
                .HasOne(s => s.User2)
                .WithMany(u => u.Divisions2)
                .HasForeignKey(s => s.UpdatedBy);

            modelBuilder.Entity<GrantDetails>()
                .HasMany(g => g.Owners)
                .WithOne(o => o.Grant)
                .HasForeignKey(o => o.GrantId);

            modelBuilder.Entity<GrantDetails>()
            .HasMany(d => d.Owners)
            .WithOne(c => c.Grant)
            .HasForeignKey(c => c.GrantId);

            modelBuilder.Entity<OwnerDetails>()
                .HasOne(s => s.OwnerType)
                .WithMany(u => u.Owners)
                .HasForeignKey(s => s.OwnerTypeId);

            modelBuilder.Entity<GrantDetails>()
                .HasOne(s => s.ProjectType)
                .WithMany(u => u.Grants)
                .HasForeignKey(s => s.ProjectTypeId);

            modelBuilder.Entity<GrantDetails>()
                .HasOne(s => s.Village)
                .WithMany(u => u.Grants)
                .HasForeignKey(s => s.VillageID);

            modelBuilder.Entity<GrantDetails>()
                .HasOne(s => s.NocPermissionType)
                .WithMany(u => u.Grants)
                .HasForeignKey(s => s.NocPermissionTypeID);

            modelBuilder.Entity<GrantDetails>()
                .HasOne(s => s.NocType)
                .WithMany(u => u.Grants)
                .HasForeignKey(s => s.NocTypeId);

            modelBuilder.Entity<RecommendationDetail>().HasData(
                new RecommendationDetail{ Id = 1, Name = "Approved", Code = "A" },
                new RecommendationDetail { Id = 2, Name = "Rejected", Code = "R" },
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
                new DaysCheckMaster { Id = 6, IsRelatedToForward=1,IsRelatedToIssue=0,CheckFor = "XEN/DWS", Code = "D", NoOfDays = 1,UserRoleID=83 },
                new DaysCheckMaster { Id = 7, IsRelatedToForward=1,IsRelatedToIssue=0,CheckFor = "Principal Secretary", Code = "PS", NoOfDays = 1,UserRoleID=6 },
                new DaysCheckMaster { Id = 8, IsRelatedToForward=1,IsRelatedToIssue=0,CheckFor = "Superintending Engineer", Code = "CO", NoOfDays = 2,UserRoleID=8 }
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
                new SiteUnitMaster { Id = 1,SiteAreaUnitId=1, UnitName = "Biswa",UnitCode="M",UnitValue= 0.0125,Timesof=1, DivideBy=1 },
                new SiteUnitMaster { Id = 2, SiteAreaUnitId = 1, UnitName = "Bigha", UnitCode = "K", UnitValue = 0.25, Timesof = 1, DivideBy = 1 },
                new SiteUnitMaster { Id = 3, SiteAreaUnitId = 1, UnitName = "Biswansi", UnitCode = "S", UnitValue = 0.000625, Timesof = 1, DivideBy = 1 },
                new SiteUnitMaster { Id = 4, SiteAreaUnitId = 3, UnitName = "Biswa", UnitCode = "M", UnitValue = 0.0125, Timesof = 3, DivideBy = 1 },
                new SiteUnitMaster { Id = 5, SiteAreaUnitId = 3, UnitName = "Bigha", UnitCode = "K", UnitValue = 0.25, Timesof = 3, DivideBy = 1 },
                new SiteUnitMaster { Id = 6, SiteAreaUnitId = 3, UnitName = "Biswansi", UnitCode = "S", UnitValue = 0.000625, Timesof = 3, DivideBy = 1 },
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
            //modelBuilder.Entity<GrantApprovalMaster>().HasData(
            //    new GrantApprovalMaster { Id = 1, Name = "Pending",Code="P" },
            //    new GrantApprovalMaster { Id = 2, Name = "Reject",Code="R" },
            //    new GrantApprovalMaster { Id = 3, Name = "Forward", Code = "F" }
            //    new GrantApprovalMaster { Id = 4, Name = "Approved", Code = "A" }
            //    );

            base.OnModelCreating(modelBuilder);
        }

    }
}
