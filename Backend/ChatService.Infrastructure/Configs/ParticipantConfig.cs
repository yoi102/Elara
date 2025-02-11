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

        builder.HasOne<Participant>()
       .WithMany()
       .HasForeignKey(e => e.ConversationId)
       .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(m => new { m.ConversationId, m.UserId })
       .IsUnique();
    }
}
