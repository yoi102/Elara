using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalSpaceService.Domain.Entities;

namespace PersonalSpaceService.Infrastructure.Configs;

internal class ContactRequestConfig : IEntityTypeConfiguration<ContactRequest>
{
    public void Configure(EntityTypeBuilder<ContactRequest> builder)
    {
        builder.ToTable("T_ContactRequests");
        builder.HasKey(c => c.Id);

        builder.HasIndex(c => new { c.SenderId, c.ReceiverId })
               .IsUnique(); 

        builder.Property(c => c.ReceiverId)
               .IsRequired();
        builder.Property(c => c.SenderId)
               .IsRequired();
    }
}
