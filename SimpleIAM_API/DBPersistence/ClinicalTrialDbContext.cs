using Microsoft.EntityFrameworkCore;
using SimpleIAM_API.Entity;

namespace SimpleIAM_API.DBPersistence
{
    public class ClinicalTrialDbContext : DbContext
    {
        public ClinicalTrialDbContext(DbContextOptions<ClinicalTrialDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>().HasData(
                new Group { Id = 1, Name = "Investigator" },
                new Group { Id = 2, Name = "Subinvestigator" },
                new Group { Id = 3, Name = "DataManager" },
                new Group { Id = 4, Name = "Monitor" },
                new Group { Id = 5, Name = "CRA" }
            );

            // Define composite key for UserGroup
            modelBuilder.Entity<UserGroup>()
                .HasKey(ug => new { ug.UserId, ug.GroupId });

            base.OnModelCreating(modelBuilder);
        }

    }
}
