using ChatService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatService.Infrastructure.Configs;
internal class GroupConversationMemberConfig : IEntityTypeConfiguration<GroupConversationMember>
{
    public void Configure(EntityTypeBuilder<GroupConversationMember> builder)
    {
        builder.ToTable("T_GroupConversationMembers");
        builder.HasKey(e => e.Id);

        builder.HasOne<GroupConversationMember>()
       .WithMany()
       .HasForeignKey(e => e.GroupConversationId)
       .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(m => new { m.GroupConversationId, m.UserId })
       .IsUnique();
    }
}
