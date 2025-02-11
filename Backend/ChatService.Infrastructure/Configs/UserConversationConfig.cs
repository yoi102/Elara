using ChatService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatService.Infrastructure.Configs;

internal class UserConversationConfig : IEntityTypeConfiguration<UserConversation>
{
    public void Configure(EntityTypeBuilder<UserConversation> builder)
    {
        builder.ToTable("T_UserConversations");
        builder.HasKey(e => e.Id);

        builder.HasOne<Conversation>()
               .WithMany()
               .HasForeignKey(uc => uc.ConversationId)
               .OnDelete(DeleteBehavior.Cascade);// 联级删除
    }
}
