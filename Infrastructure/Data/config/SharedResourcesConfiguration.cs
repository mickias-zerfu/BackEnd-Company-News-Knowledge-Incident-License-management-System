using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.config
{
    public class SharedResourcesConfiguration: IEntityTypeConfiguration<SharedResource>
    {
        public void Configure(EntityTypeBuilder<SharedResource> builder)
        { 
             
        builder.HasKey(sr => sr.Id);
        builder.Property(sr => sr.FileTitle).IsRequired();
        builder.Property(sr => sr.FileName).IsRequired();
        builder.Property(sr => sr.FileDescription).IsRequired();
        builder.Property(sr => sr.FileType);
        builder.Property(sr => sr.FileData);
        builder.Property(sr => sr.Created_at);
        builder.Property(sr => sr.Updated_at);
        }
    }
}