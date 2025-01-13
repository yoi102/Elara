using ChatService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatService.Infrastructure.Configs;

internal class PersonalConversationConfig : IEntityTypeConfiguration<PersonalConversation>
{
    public void Configure(EntityTypeBuilder<PersonalConversation> builder)
    {
        builder.ToTable("T_PersonalConversations");
        builder.HasKey(e => e.Id);

        builder.HasIndex(m => new { m.User1Id, m.User2Id })
       .IsUnique();
    }
}
