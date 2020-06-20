﻿// <auto-generated />

namespace Perigee.Framework.Data.EntityFramework.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Metadata;

    [DbContext(typeof(EntityDbContext))]
    internal class EntityDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Lofty.Entities.Address", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:ValueGenerationStrategy",
                        SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("Address1")
                    .IsRequired()
                    .HasColumnType("nvarchar(150)")
                    .HasMaxLength(150);

                b.Property<string>("Address2")
                    .HasColumnType("nvarchar(150)")
                    .HasMaxLength(150);

                b.Property<string>("Country")
                    .IsRequired()
                    .HasColumnType("nvarchar(30)")
                    .HasMaxLength(30);

                b.Property<string>("CreatedBy")
                    .IsRequired()
                    .HasColumnType("nvarchar(100)")
                    .IsUnicode();

                b.Property<DateTime?>("CreatedOn")
                    .IsRequired()
                    .HasColumnType("datetime2");

                b.Property<int>("CustomerId");

                b.Property<bool>("IsCurrent")
                    .HasColumnType("bit");

                b.Property<string>("PostCode")
                    .IsRequired()
                    .HasColumnType("nvarchar(15)")
                    .HasMaxLength(15);

                b.Property<string>("State")
                    .IsRequired()
                    .HasColumnType("nvarchar(30)")
                    .HasMaxLength(30);

                b.Property<string>("Suburb")
                    .IsRequired()
                    .HasColumnType("nvarchar(30)")
                    .HasMaxLength(30);

                b.Property<string>("UpdatedBy")
                    .IsRequired()
                    .HasColumnType("nvarchar(100)")
                    .IsUnicode();

                b.Property<DateTime?>("UpdatedOn")
                    .IsRequired()
                    .HasColumnType("datetime2");

                b.HasKey("Id");

                b.HasIndex("CustomerId");

                b.ToTable("Address");
            });

            modelBuilder.Entity("Lofty.Entities.Customer", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:ValueGenerationStrategy",
                        SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("CreatedBy")
                    .IsRequired()
                    .HasColumnType("nvarchar(100)")
                    .IsUnicode();

                b.Property<DateTime?>("CreatedOn")
                    .IsRequired()
                    .HasColumnType("datetime2");

                b.Property<string>("EmailAddress")
                    .HasColumnType("nvarchar(255)")
                    .HasMaxLength(255);

                b.Property<string>("FirstName")
                    .IsRequired()
                    .HasColumnType("nvarchar(100)")
                    .HasMaxLength(100);

                b.Property<string>("LastName")
                    .IsRequired()
                    .HasColumnType("nvarchar(100)")
                    .HasMaxLength(100);

                b.Property<string>("UpdatedBy")
                    .IsRequired()
                    .HasColumnType("nvarchar(100)")
                    .IsUnicode();

                b.Property<DateTime?>("UpdatedOn")
                    .IsRequired()
                    .HasColumnType("datetime2");

                b.HasKey("Id");

                b.ToTable("Customer");
            });

            modelBuilder.Entity("Lofty.Entities.Job", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:ValueGenerationStrategy",
                        SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<DateTime>("BookedDateTime")
                    .HasColumnType("datetime2");

                b.Property<string>("Comments");

                b.Property<string>("CreatedBy")
                    .IsRequired()
                    .HasColumnType("nvarchar(100)")
                    .IsUnicode();

                b.Property<DateTime?>("CreatedOn")
                    .IsRequired()
                    .HasColumnType("datetime2");

                b.Property<int>("CustomerId");

                b.Property<DateTime?>("DeliveredDateTime")
                    .HasColumnType("datetime2");

                b.Property<string>("EquipmentReceivedDescription");

                b.Property<DateTime?>("GearReceivedDateTime")
                    .HasColumnType("datetime2");

                b.Property<bool>("IsDeleted")
                    .HasColumnType("bit");

                b.Property<string>("JobId")
                    .IsRequired()
                    .HasMaxLength(1000);

                b.Property<int>("JobTypeId");

                b.Property<string>("UpdatedBy")
                    .IsRequired()
                    .HasColumnType("nvarchar(100)")
                    .IsUnicode();

                b.Property<DateTime?>("UpdatedOn")
                    .IsRequired()
                    .HasColumnType("datetime2");

                b.HasKey("Id");

                b.HasIndex("CustomerId");

                b.ToTable("Job");
            });

            modelBuilder.Entity("Lofty.Entities.JobType", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:ValueGenerationStrategy",
                        SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(100);

                b.HasKey("Id");

                b.ToTable("JobType");
            });

            modelBuilder.Entity("Lofty.Entities.PhoneNumber", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:ValueGenerationStrategy",
                        SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("CreatedBy")
                    .IsRequired()
                    .HasColumnType("nvarchar(100)")
                    .IsUnicode();

                b.Property<DateTime?>("CreatedOn")
                    .IsRequired()
                    .HasColumnType("datetime2");

                b.Property<int>("CustomerId");

                b.Property<bool>("IsCurrent")
                    .HasColumnType("bit");

                b.Property<bool>("IsMobile")
                    .HasColumnType("bit");

                b.Property<string>("Number")
                    .IsRequired()
                    .HasMaxLength(20);

                b.Property<bool>("ReceiveTextMessages")
                    .HasColumnType("bit");

                b.Property<string>("UpdatedBy")
                    .IsRequired()
                    .HasColumnType("nvarchar(100)")
                    .IsUnicode();

                b.Property<DateTime?>("UpdatedOn")
                    .IsRequired()
                    .HasColumnType("datetime2");

                b.HasKey("Id");

                b.HasIndex("CustomerId");

                b.ToTable("PhoneNumber");
            });

            modelBuilder.Entity("Lofty.Entities.Address", b =>
            {
                b.HasOne("Lofty.Entities.Customer", "Customer")
                    .WithMany("Addresses")
                    .HasForeignKey("CustomerId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Lofty.Entities.Job", b =>
            {
                b.HasOne("Lofty.Entities.Customer", "Customer")
                    .WithMany("Jobs")
                    .HasForeignKey("CustomerId")
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne("Lofty.Entities.JobType", "JobType")
                    .WithMany("Jobs")
                    .HasForeignKey("CustomerId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Lofty.Entities.PhoneNumber", b =>
            {
                b.HasOne("Lofty.Entities.Customer", "Customer")
                    .WithMany("PhoneNumbers")
                    .HasForeignKey("CustomerId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
#pragma warning restore 612, 618
        }
    }
}