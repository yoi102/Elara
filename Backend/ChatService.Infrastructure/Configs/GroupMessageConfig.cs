using ChatService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatService.Infrastructure.Configs;

internal class GroupMessageConfig : IEntityTypeConfiguration<GroupMessage>
{
    public void Configure(EntityTypeBuilder<GroupMessage> builder)
    {
        builder.ToTable("T_GroupMessages");
        builder.HasKey(e => e.Id);

        builder.Navigation(e => e.Attachments)
               .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasOne<GroupConversation>()
               .WithMany()
               .HasForeignKey(e => e.GroupConversationId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
