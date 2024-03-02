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
        public DbSet<DrainCoordinatesDetails> DrainCoordinatesDetails { get; set; }
        public DbSet<DrainDetails> DrainDetails { get; set; }
        public DbSet<OwnerTypeDetails> OwnerTypeDetails { get; set; }
        public DbSet<OwnerDetails> OwnerDetails { get; set; }
        public DbSet<NocPermissionTypeDetails> NocPermissionTypeDetails { get; set; }
        public DbSet<NocTypeDetails> NocTypeDetails { get; set; }
        public DbSet<ProjectTypeDetails> ProjectTypeDetails { get; set; }
        public DbSet<GrantDetails> GrantDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Village)
                .WithMany(v => v.ApplicationUsers)
                .HasForeignKey(u => u.VillageId)
                .OnDelete(DeleteBehavior.Restrict); // Use appropriate delete behavior based on your requirements

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.TehsilBlock)
                .WithMany(c => c.ApplicationUsers)
                .HasForeignKey(u => u.TehsilBlockId)
                .OnDelete(DeleteBehavior.Restrict); // Use appropriate delete behavior based on your requirements

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.SubDivision)
                .WithMany(c => c.ApplicationUsers)
                .HasForeignKey(u => u.SubDivisionId)
                .OnDelete(DeleteBehavior.Restrict); // Use appropriate delete behavior based on your requirements

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Division)
                .WithMany(c => c.ApplicationUsers)
                .HasForeignKey(u => u.DivisionId)
                .OnDelete(DeleteBehavior.Restrict); // Use appropriate delete behavior based on your requirements

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

            modelBuilder.Entity<DrainCoordinatesDetails>()
                .HasOne(s => s.User)
                .WithMany(u => u.DrainCoordinates)
                .HasForeignKey(s => s.CreatedBy);

            modelBuilder.Entity<DrainCoordinatesDetails>().Ignore(d => d.User2);

            modelBuilder.Entity<DrainDetails>()
            .HasMany(d => d.DrainCoordinates)
            .WithOne(c => c.Drain)
            .HasForeignKey(c => c.DrainId);

            modelBuilder.Entity<DrainDetails>()
                .HasOne(s => s.User)
                .WithMany(u => u.Drains)
                .HasForeignKey(s => s.CreatedBy);

            modelBuilder.Entity<DrainDetails>().Ignore(d => d.User2);
            modelBuilder.Entity<DrainDetails>()
                .HasOne(s => s.User2)
                .WithMany(u => u.Drains2)
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

            base.OnModelCreating(modelBuilder);
        }

    }
}
