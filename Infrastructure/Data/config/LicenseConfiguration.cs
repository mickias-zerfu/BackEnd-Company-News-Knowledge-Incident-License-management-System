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
            builder.Property(l => l.IssuedTo).IsRequired();
            builder.Property(l => l.IssuedBy).IsRequired();
            builder.Property(l => l.CreationDate).IsRequired();
            builder.Property(l => l.ExpirationDate).IsRequired();
            builder.Property(l => l.MaxUsers).IsRequired();
            builder.Property(l => l.Activated).IsRequired();
            builder.Property(l => l.LicenseType).IsRequired();

            // Configure the SoftwareProductId property
            builder.Property(l => l.SoftwareProductId)
                .IsRequired();

            // Relationships
            builder.HasOne<SoftwareProduct>()
                .WithMany()
                .HasForeignKey(l => l.SoftwareProductId)
                .IsRequired(); 
            // Relationship
            builder.HasMany(l => l.LicenseManagers)
                .WithMany(lm => lm.Licenses)
                .UsingEntity<LicenseManagerLicense>(
                    j => j.HasOne(lml => lml.LicenseManager)
                        .WithMany()
                        .HasForeignKey(lml => lml.LicenseManagerId),
                    j => j.HasOne(lml => lml.License)
                        .WithMany()
                        .HasForeignKey(lml => lml.LicenseId),
                    j =>
                    {
                        j.HasKey(lml => new { lml.LicenseManagerId, lml.LicenseId });
                    });
        
        
        }
    }

    public class SoftwareProductConfiguration : IEntityTypeConfiguration<SoftwareProduct>
    {
        public void Configure(EntityTypeBuilder<SoftwareProduct> builder)
        {
            // Primary key
            builder.HasKey(sp => sp.Id);

            // Properties
            builder.Property(sp => sp.Name).IsRequired();
            builder.Property(sp => sp.Version).IsRequired();
            builder.Property(sp => sp.Description).IsRequired();
            builder.Property(sp => sp.Vendor).IsRequired();
            builder.Property(sp => sp.ReleaseDate).IsRequired();

            // Relationships 
        }
    }

    public class LicenseManagerConfiguration : IEntityTypeConfiguration<LicenseManager>
    {
        public void Configure(EntityTypeBuilder<LicenseManager> builder)
        {
            // Primary key
            builder.HasKey(u => u.Id);

            // Properties
            builder.Property(u => u.Email).IsRequired();
            builder.Property(u => u.FirstName).IsRequired();
            builder.Property(u => u.LastName).IsRequired();
            builder.Property(u => u.Role).IsRequired();
            builder.Property(u => u.IsActive).IsRequired();
            builder.Property(u => u.RegistrationDate).IsRequired();
            builder.Property(u => u.PhoneNumber).IsRequired();
            builder.Property(u => u.ProfilePictureUrl).IsRequired(false);;

            builder.HasMany(lm => lm.Licenses)
            .WithMany(l => l.LicenseManagers)
            .UsingEntity<LicenseManagerLicense>(
                j => j.HasOne(lml => lml.License)
                    .WithMany()
                    .HasForeignKey(lml => lml.LicenseId),
                j => j.HasOne(lml => lml.LicenseManager)
                    .WithMany()
                    .HasForeignKey(lml => lml.LicenseManagerId),
                j =>
                {
                    j.HasKey(lml => new { lml.LicenseManagerId, lml.LicenseId });
                });                       
        }
    }
    public class LicenseManagerLicenseConfiguration : IEntityTypeConfiguration<LicenseManagerLicense>
    {
        public void Configure(EntityTypeBuilder<LicenseManagerLicense> builder)
        {

            builder.HasKey(lml => new { lml.LicenseManagerId, lml.LicenseId }); 
        }
    }
}