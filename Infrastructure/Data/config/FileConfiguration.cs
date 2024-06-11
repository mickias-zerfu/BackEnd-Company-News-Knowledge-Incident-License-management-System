using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.config
{
  public class FileConfiguration : IEntityTypeConfiguration<FileDetails>
{
    public void Configure(EntityTypeBuilder<FileDetails> builder)
    {
        builder.ToTable("FileDetails"); // Set the table name

        builder.HasKey(e => e.ID); // Set the primary key

        builder.Property(e => e.ID)
            .HasColumnName("ID")
            .ValueGeneratedOnAdd(); // Configure the ID property

        builder.Property(e => e.FileName)
            .HasColumnName("FileName")
            .IsRequired()
            .HasMaxLength(30); // Configure the FileName property

        builder.Property(e => e.FileData)
            .HasColumnName("FileData")
            .IsRequired(); // Configure the FileData property

        builder.Property(e => e.FileType)
            .HasColumnName("FileType"); // Configure the FileType property
    }
}
}