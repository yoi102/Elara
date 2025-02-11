using ChatService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatService.Infrastructure.Configs;

internal class ConversationConfig : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder.ToTable("T_Conversations");
        builder.HasKey(e => e.Id);

        builder.HasIndex(c => new { c.Name, c.IsGroup })
               .IsUnique()
               .HasFilter("[IsGroup] = 1"); // Name只对群组聊天唯一
    }
}
