﻿// <auto-generated />
using System;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace WebAPIBook.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    [Migration("20220202102814_InitialData")]
    partial class InitialData
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Entities.Models.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CompanyId");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("Id");

                    b.ToTable("Companies");

                    b.HasData(
                        new
                        {
                            Id = new Guid("9d2139e2-ae97-4ee5-95f4-e8769f7fd70f"),
                            Address = "Address test 123",
                            Country = "USA",
                            Name = "IT Company"
                        },
                        new
                        {
                            Id = new Guid("c4952391-bbc9-47e2-94b7-e50ce255fe26"),
                            Address = "Adress 2 test 123",
                            Country = "EU",
                            Name = "Admin Company"
                        });
                });

            modelBuilder.Entity("Entities.Models.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("EmployeeId");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            Id = new Guid("39317070-8da4-4b8e-adfc-03a46b31d4b9"),
                            Age = 26,
                            CompanyId = new Guid("9d2139e2-ae97-4ee5-95f4-e8769f7fd70f"),
                            Name = "Sam Raiden",
                            Position = "Software Developer"
                        },
                        new
                        {
                            Id = new Guid("42dc3e81-1e45-415a-8889-09fef70d08f2"),
                            Age = 30,
                            CompanyId = new Guid("9d2139e2-ae97-4ee5-95f4-e8769f7fd70f"),
                            Name = "Jana McLeaf",
                            Position = "Software Developer"
                        },
                        new
                        {
                            Id = new Guid("6000dc78-6cc1-489d-8c18-2a82b42115e2"),
                            Age = 35,
                            CompanyId = new Guid("c4952391-bbc9-47e2-94b7-e50ce255fe26"),
                            Name = "KaneMiller",
                            Position = "Software Developer"
                        });
                });

            modelBuilder.Entity("Entities.Models.Employee", b =>
                {
                    b.HasOne("Entities.Models.Company", "Company")
                        .WithMany("Employees")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Entities.Models.Company", b =>
                {
                    b.Navigation("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}