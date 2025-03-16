﻿// <auto-generated />
using System;
using FileService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FileService.Infrastructure.Migrations
{
    [DbContext(typeof(FileServiceDbContext))]
    partial class FileServiceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("FileService.Domain.Entities.UploadedItem", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("BackupUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Filename")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .IsUnicode(true)
                        .HasColumnType("TEXT");

                    b.Property<string>("FileSHA256Hash")
                        .IsRequired()
                        .HasMaxLength(64)
                        .IsUnicode(false)
                        .HasColumnType("TEXT");

                    b.Property<long>("FileSizeInBytes")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RemoteUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FileSHA256Hash", "FileSizeInBytes");

                    b.ToTable("FileService_UploadedItems", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
