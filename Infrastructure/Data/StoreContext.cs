
using System.Linq;
using System.Reflection;
using Core.Entities;
using Core.Entities.licenseEntity;
using Infrastructure.Data.config;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<News> News { get; set; }
        public DbSet<KnowledgeBase> KnowledgeBases { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<SharedResource> SharedResources { get; set; }
        public DbSet<FileDetails> FileDetails { get; set; }
        public DbSet<Comment> Comments { get; internal set; }


        public DbSet<License> Licenses { get; set; }
        public DbSet<SoftwareProduct> SoftwareProducts { get; set; }
        public DbSet<LicenseManager> LicenseManagers { get; set; }
        public DbSet<LicenseManagerLicense> LicenseManagerLicenses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new NewsConfiguration());
            modelBuilder.ApplyConfiguration(new KnowledgeBaseConfiguration());
            modelBuilder.ApplyConfiguration(new IncidentConfiguration());
            modelBuilder.ApplyConfiguration(new SharedResourcesConfiguration());
            modelBuilder.ApplyConfiguration(new FileConfiguration());


            modelBuilder.ApplyConfiguration(new LicenseConfiguration());
            modelBuilder.ApplyConfiguration(new SoftwareProductConfiguration());
            modelBuilder.ApplyConfiguration(new LicenseManagerConfiguration());
            modelBuilder.ApplyConfiguration(new LicenseManagerLicenseConfiguration());

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}