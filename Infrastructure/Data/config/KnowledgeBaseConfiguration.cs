using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.config
{
    public class KnowledgeBaseConfiguration : IEntityTypeConfiguration<KnowledgeBase>
    {
        public void Configure(EntityTypeBuilder<KnowledgeBase> builder)
        {
        builder.HasKey(k => k.Id);
        builder.Property(k => k.Problem).IsRequired();
        builder.Property(k => k.ProblemDescription).IsRequired(); 
        }
    }
} 

