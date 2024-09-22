using ChatService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatService.Infrastructure.Configs;

internal class MessageConfig : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("T_Messages");
        builder.HasKey(e => e.Id);

        builder.HasOne<Conversation>()
               .WithMany()
               .HasForeignKey(e => e.ConversationId)
               .OnDelete(DeleteBehavior.Cascade);// 联级删除
    }
}
