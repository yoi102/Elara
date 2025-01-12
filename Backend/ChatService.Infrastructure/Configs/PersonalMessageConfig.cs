using ChatService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatService.Infrastructure.Configs;

internal class PersonalMessageConfig : IEntityTypeConfiguration<PersonalMessage>
{
    public void Configure(EntityTypeBuilder<PersonalMessage> builder)
    {
        builder.ToTable("T_PersonalMessages");
        builder.HasKey(e => e.Id);

        builder.Navigation(e => e.Attachments)
               .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasOne<PersonalConversation>()
               .WithMany()
               .HasForeignKey(e => e.PersonalConversationId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
