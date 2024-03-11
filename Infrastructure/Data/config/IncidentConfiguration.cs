using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.config
{
    public class IncidentConfiguration : IEntityTypeConfiguration<Incident>
    {
        public void Configure(EntityTypeBuilder<Incident> builder)
        {

            builder.HasKey(i => i.Id);
            builder.Property(i => i.IncidentTitle).IsRequired();
            builder.Property(i => i.IncidentDescription).IsRequired();
            builder.Property(e => e.StatusAction)
        .HasConversion(
            v => string.Join(",", v),
            v => v.Split(",", StringSplitOptions.RemoveEmptyEntries));
            builder.Property(e => e.QuickReviews)
        .HasConversion(
            v => string.Join(",", v),
            v => v.Split(",", StringSplitOptions.RemoveEmptyEntries));
            builder.Property(e => e.SolutionToIncident)
        .HasConversion(
            v => string.Join(",", v),
            v => v.Split(",", StringSplitOptions.RemoveEmptyEntries));
            builder.Property(i => i.Remark);
        }
    }
}