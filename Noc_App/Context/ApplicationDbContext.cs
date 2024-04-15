using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Noc_App.Models;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<SiteAreaUnitDetails>().HasData(
                new SiteAreaUnitDetails { Id = 1, Name = "Bigha/Biswa/Biswansi" },
                new SiteAreaUnitDetails { Id = 2, Name = "Kanal/Marla/Sarsai" }
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

            base.OnModelCreating(modelBuilder);
        }

    }
}
