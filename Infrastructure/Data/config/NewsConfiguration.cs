using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.config
{
    public class NewsConfiguration : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> builder)
        {
            builder.HasKey(n => n.Id);
            builder.Property(n => n.Id).IsRequired();
            builder.Property(n => n.Title).IsRequired().HasMaxLength(250);
            builder.Property(n => n.Content).IsRequired();
            builder.HasMany(n => n.Comments).WithOne(c => c.News)
            .HasForeignKey(c => c.NewsPostId)
            .OnDelete(DeleteBehavior.Cascade);
        }
        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     base.OnModelCreating(modelBuilder);
        // }
    }
}