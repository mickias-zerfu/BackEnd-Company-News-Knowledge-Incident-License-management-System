﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    [DbContext(typeof(StoreContext))]
    partial class StoreContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.13");

            modelBuilder.Entity("Core.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("NewsPostId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NewsPostId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Core.Entities.FileDetails", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("ID");

                    b.Property<byte[]>("FileData")
                        .IsRequired()
                        .HasColumnType("BLOB")
                        .HasColumnName("FileData");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("TEXT")
                        .HasColumnName("FileName");

                    b.Property<int>("FileType")
                        .HasColumnType("INTEGER")
                        .HasColumnName("FileType");

                    b.HasKey("ID");

                    b.ToTable("FileDetails", (string)null);
                });

            modelBuilder.Entity("Core.Entities.Incident", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Created_at")
                        .HasColumnType("TEXT");

                    b.Property<string>("IncidentDescription")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("IncidentTitle")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("QuickReviews")
                        .HasColumnType("TEXT");

                    b.Property<string>("Remark")
                        .HasColumnType("TEXT");

                    b.Property<string>("SolutionToIncident")
                        .HasColumnType("TEXT");

                    b.Property<string>("StatusAction")
                        .HasColumnType("TEXT");

                    b.Property<string>("Updated_at")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Incidents");
                });

            modelBuilder.Entity("Core.Entities.KnowledgeBase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Created_at")
                        .HasColumnType("TEXT");

                    b.Property<string>("Problem")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ProblemDescription")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Solution")
                        .HasColumnType("TEXT");

                    b.Property<string>("Updated_at")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("KnowledgeBases");
                });

            modelBuilder.Entity("Core.Entities.News", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Created_at")
                        .HasColumnType("TEXT");

                    b.Property<string>("Image_url")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("TEXT");

                    b.Property<string>("Updated_at")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("News");
                });

            modelBuilder.Entity("Core.Entities.SharedResource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Created_at")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("FileData")
                        .HasColumnType("BLOB");

                    b.Property<string>("FileDescription")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FileTitle")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("FileType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Updated_at")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("SharedResources");
                });

            modelBuilder.Entity("Core.Entities.licenseEntity.License", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Activated")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("ActivationDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("IssuedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("IssuedTo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("LicenseType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaxUsers")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("SoftwareProductId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SoftwareProductId");

                    b.ToTable("Licenses");
                });

            modelBuilder.Entity("Core.Entities.licenseEntity.LicenseManager", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ProfilePictureUrl")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("LicenseManagers");
                });

            modelBuilder.Entity("Core.Entities.licenseEntity.LicenseManagerLicense", b =>
                {
                    b.Property<int>("LicenseManagerId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LicenseId")
                        .HasColumnType("INTEGER");

                    b.HasKey("LicenseManagerId", "LicenseId");

                    b.HasIndex("LicenseId");

                    b.ToTable("LicenseManagerLicenses");
                });

            modelBuilder.Entity("Core.Entities.licenseEntity.SoftwareProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Vendor")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("SoftwareProducts");
                });

            modelBuilder.Entity("Core.Entities.Comment", b =>
                {
                    b.HasOne("Core.Entities.News", "News")
                        .WithMany("Comments")
                        .HasForeignKey("NewsPostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("News");
                });

            modelBuilder.Entity("Core.Entities.licenseEntity.License", b =>
                {
                    b.HasOne("Core.Entities.licenseEntity.SoftwareProduct", null)
                        .WithMany()
                        .HasForeignKey("SoftwareProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Entities.licenseEntity.LicenseManagerLicense", b =>
                {
                    b.HasOne("Core.Entities.licenseEntity.License", "License")
                        .WithMany()
                        .HasForeignKey("LicenseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Entities.licenseEntity.LicenseManager", "LicenseManager")
                        .WithMany()
                        .HasForeignKey("LicenseManagerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("License");

                    b.Navigation("LicenseManager");
                });

            modelBuilder.Entity("Core.Entities.News", b =>
                {
                    b.Navigation("Comments");
                });
#pragma warning restore 612, 618
        }
    }
}
