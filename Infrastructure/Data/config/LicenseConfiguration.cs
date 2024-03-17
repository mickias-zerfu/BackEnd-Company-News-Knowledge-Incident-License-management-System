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

            // Relationships
            builder.HasOne(l => l.SoftwareProduct)
                .WithMany(sp => sp.Licenses)
                .HasForeignKey(l => l.SoftwareProductId)
                .IsRequired();
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
            builder.HasMany(sp => sp.Licenses)
                .WithOne(l => l.SoftwareProduct)
                .HasForeignKey(l => l.SoftwareProductId)
                .IsRequired();
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
            builder.Property(u => u.ProfilePictureUrl);

            // Relationships
            builder.HasOne(u => u.License)
                .WithMany()
                .HasForeignKey(u => u.LicenseId)
                .IsRequired();
        }
    }
}
