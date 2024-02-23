using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.config
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).IsRequired();
            builder.Property(c => c.Text).IsRequired();
            builder.Property(c => c.CreatedAt).IsRequired();

            // Configure foreign key relationship with News entity
            builder.HasOne(c => c.News)
                .WithMany(n => n.Comments)
                .HasForeignKey(c => c.NewsPostId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
