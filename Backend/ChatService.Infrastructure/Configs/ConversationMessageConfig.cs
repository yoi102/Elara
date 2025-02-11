using ChatService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatService.Infrastructure.Configs;

internal class ConversationMessageConfig : IEntityTypeConfiguration<ConversationMessage>
{
    public void Configure(EntityTypeBuilder<ConversationMessage> builder)
    {
        builder.ToTable("T_ConversationMessages");
        builder.HasKey(e => e.Id);

        builder.Navigation(e => e.Attachments)
               .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasOne<Conversation>()
               .WithMany()
               .HasForeignKey(e => e.ConversationId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
