using Core.Entities.licenseEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.config
{
    public class LicenseConfiguration : IEntityTypeConfiguration<License>
    {
        public void Configure(EntityTypeBuilder<License> builder)
        {
            // Primary key
            builder.HasKey(l => l.Id);

            // Properties
            builder.Property(l => l.IssuedTo)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(l => l.IssuedBy)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(l => l.CreationDate)
                   .IsRequired();

            builder.Property(l => l.ActivationDate);

            builder.Property(l => l.ExpirationDate)
                   .IsRequired();

            builder.Property(l => l.MaxUsers)
                   .IsRequired();

            builder.Property(l => l.Activated)
                   .IsRequired();

            builder.Property(l => l.LicenseType)
                   .IsRequired()
                   .HasConversion<int>(); // Store enum as int

            builder.Property(l => l.Notes)
                   .HasMaxLength(500); // Assuming a reasonable max length for notes

            // Relationships
            builder.HasOne<SoftwareProduct>()
                   .WithMany() // No inverse navigation property in SoftwareProduct
                   .HasForeignKey(l => l.SoftwareProductId)
                   .OnDelete(DeleteBehavior.Cascade); // Cascade delete

            // Relationships for LicenseManagerLicenses
            builder.HasMany(l => l.LicenseManagerLicenses)
                   .WithOne(lm => lm.License)
                   .HasForeignKey(lm => lm.LicenseId)
                   .OnDelete(DeleteBehavior.Cascade); // Cascade delete for related LicenseManagerLicenses

            // Indexes
            builder.HasIndex(l => l.ExpirationDate);
        }
    }

    public class SoftwareProductConfiguration : IEntityTypeConfiguration<SoftwareProduct>
    {
        public void Configure(EntityTypeBuilder<SoftwareProduct> builder)
        {
            // Primary key
            builder.HasKey(sp => sp.Id);

            // Properties
            builder.Property(sp => sp.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(sp => sp.Version)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(sp => sp.Description)
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.Property(sp => sp.Vendor)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(sp => sp.ReleaseDate)
                   .IsRequired();

            // Relationships
            builder.HasMany<License>()
                   .WithOne()
                   .HasForeignKey(l => l.SoftwareProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(sp => sp.Name);
        }
    }

    public class LicenseManagerConfiguration : IEntityTypeConfiguration<LicenseManager>
    {
        public void Configure(EntityTypeBuilder<LicenseManager> builder)
        {
            // Primary key
            builder.HasKey(u => u.Id);

            // Properties
            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.HasIndex(u => u.Email)
                   .IsUnique(); // Ensure email uniqueness

            builder.Property(u => u.FirstName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.LastName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.Role)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(u => u.IsActive)
                   .IsRequired();

            builder.Property(u => u.RegistrationDate)
                   .IsRequired();

            builder.Property(u => u.PhoneNumber)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(u => u.ProfilePictureUrl)
                   .HasMaxLength(200); // Assuming a reasonable max length for URL

            // Relationships
            builder.HasMany<LicenseManagerLicense>()
                   .WithOne(lm => lm.LicenseManager)
                   .HasForeignKey(lm => lm.LicenseManagerId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(u => u.PhoneNumber);
        }
    }

    public class LicenseManagerLicenseConfiguration : IEntityTypeConfiguration<LicenseManagerLicense>
    {
        public void Configure(EntityTypeBuilder<LicenseManagerLicense> builder)
        {
            // Composite primary key
            builder.HasKey(lm => new { lm.LicenseId, lm.LicenseManagerId });

            // Relationships
            builder.HasOne(lm => lm.License)
                   .WithMany(l => l.LicenseManagerLicenses)
                   .HasForeignKey(lm => lm.LicenseId)
                   .OnDelete(DeleteBehavior.Cascade); // Cascade delete

            builder.HasOne(lm => lm.LicenseManager)
                   .WithMany(m => m.LicenseManagerLicenses)
                   .HasForeignKey(lm => lm.LicenseManagerId)
                   .OnDelete(DeleteBehavior.Cascade); // Cascade delete

            // Indexes for performance optimization
            builder.HasIndex(lm => lm.LicenseId);
            builder.HasIndex(lm => lm.LicenseManagerId);
        }
    }
}
