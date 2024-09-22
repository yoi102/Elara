using FileService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileService.Infrastructure.Configs;

internal class UploadedItemConfig : IEntityTypeConfiguration<UploadedItem>
{
    public void Configure(EntityTypeBuilder<UploadedItem> builder)
    {
        builder.ToTable("T_UploadedItems");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Filename).IsUnicode().HasMaxLength(1024);
        builder.Property(e => e.FileSHA256Hash).IsUnicode(false).HasMaxLength(64);
        //经常要按照这两个列进行查询，把两个组成复合索引，提高查询效率。
        builder.HasIndex(e => new { e.FileSHA256Hash, e.FileSizeInBytes });
    }
}