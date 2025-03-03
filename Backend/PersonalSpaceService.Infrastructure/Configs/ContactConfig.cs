using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalSpaceService.Domain.Entities;

namespace PersonalSpaceService.Infrastructure.Configs;

internal class ContactConfig : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("T_Contacts");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.OwnerId)
               .IsRequired();
    }
}
