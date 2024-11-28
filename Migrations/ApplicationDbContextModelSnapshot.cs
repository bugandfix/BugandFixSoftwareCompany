﻿// <auto-generated />
using System;
using BugandFixSoftwareCompany.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BugandFixSoftwareCompany.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BugandFixSoftwareCompany.Entity.SoftwareDeveloper", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Experience")
                        .HasColumnType("int");

                    b.Property<string>("LinkedInProfile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Specialization")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SoftwareDevelopers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Experience = 10,
                            Name = "Ali",
                            Specialization = "Backend",
                            Title = "Unknown"
                        },
                        new
                        {
                            Id = 2,
                            Experience = 3,
                            Name = "Reza",
                            Specialization = "Frontend",
                            Title = "Unknown"
                        },
                        new
                        {
                            Id = 3,
                            Experience = 12,
                            Name = "Hamid",
                            Specialization = "DevOps",
                            Title = "Unknown"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
