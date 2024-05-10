using Core.Entities.AppUser;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders; 

namespace Infrastructure.Data.Configurations
{
    public class SubadminConfiguration : IEntityTypeConfiguration<SubAdmin>
    {
        public void Configure(EntityTypeBuilder<SubAdmin> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).IsRequired();
            builder.Property(c => c.Email).IsRequired();
            builder.Property(c => c.RoleId).IsRequired();
            builder.Property(c => c.DisplayName).IsRequired();
            builder.Property(c => c.PasswordHash).IsRequired();
               builder.Property(c => c.Access)
                   .IsRequired(false) // Allow null values for Access
                   .HasConversion(
                        v => string.Join(",", v),
                        v => v.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray());
        
        }
    }
}
