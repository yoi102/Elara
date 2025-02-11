using ChatService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatService.Infrastructure.Configs;

internal class ParticipantConfig : IEntityTypeConfiguration<Participant>
{
    public void Configure(EntityTypeBuilder<Participant> builder)
    {
        builder.ToTable("T_Participants");
        builder.HasKey(e => e.Id);

        builder.HasOne<Conversation>()
               .WithMany()
               .HasForeignKey(e => e.ConversationId)
               .OnDelete(DeleteBehavior.Cascade);// 联级删除

        builder.HasIndex(m => new { m.ConversationId, m.UserId })// 一个用户只能加入一次同一个会话
               .IsUnique();
    }
}
