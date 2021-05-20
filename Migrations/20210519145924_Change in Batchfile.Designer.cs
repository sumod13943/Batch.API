﻿// <auto-generated />
using System;
using BatchAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BatchAPI.Migrations
{
    [DbContext(typeof(BatchContext))]
    [Migration("20210519145924_Change in Batchfile")]
    partial class ChangeinBatchfile
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BatchAPI.Model.ACL", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ReadGroups")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReadUsers")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ACLs");
                });

            modelBuilder.Entity("BatchAPI.Model.Attributes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<Guid?>("BatchId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BatchId");

                    b.ToTable("Attributes");
                });

            modelBuilder.Entity("BatchAPI.Model.Batch", b =>
                {
                    b.Property<Guid>("BatchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("ACLId")
                        .HasColumnType("int");

                    b.Property<int?>("BatchFileId")
                        .HasColumnType("int");

                    b.Property<string>("BusinessUnit")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("datetime2");

                    b.HasKey("BatchId");

                    b.HasIndex("ACLId");

                    b.HasIndex("BatchFileId");

                    b.ToTable("Batches");
                });

            modelBuilder.Entity("BatchAPI.Model.BatchFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileSize")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BatchFiles");
                });

            modelBuilder.Entity("BatchAPI.Model.Attributes", b =>
                {
                    b.HasOne("BatchAPI.Model.Batch", null)
                        .WithMany("Attributes")
                        .HasForeignKey("BatchId");
                });

            modelBuilder.Entity("BatchAPI.Model.Batch", b =>
                {
                    b.HasOne("BatchAPI.Model.ACL", "ACL")
                        .WithMany()
                        .HasForeignKey("ACLId");

                    b.HasOne("BatchAPI.Model.BatchFile", "BatchFile")
                        .WithMany()
                        .HasForeignKey("BatchFileId");

                    b.Navigation("ACL");

                    b.Navigation("BatchFile");
                });

            modelBuilder.Entity("BatchAPI.Model.Batch", b =>
                {
                    b.Navigation("Attributes");
                });
#pragma warning restore 612, 618
        }
    }
}
