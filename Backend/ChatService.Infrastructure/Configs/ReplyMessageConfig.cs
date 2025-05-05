using ChatService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatService.Infrastructure.Configs;

internal class ReplyMessageConfig : IEntityTypeConfiguration<ReplyMessage>
{
    public void Configure(EntityTypeBuilder<ReplyMessage> builder)
    {
        builder.ToTable("T_ReplyMessages");
        builder.HasKey(e => e.Id);

        builder.HasOne<Message>()
               .WithMany()
               .HasForeignKey(r => r.RepliedMessageId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Message>()
               .WithOne()
               .HasForeignKey<ReplyMessage>(r => r.OriginalMessageId)
               .OnDelete(DeleteBehavior.Cascade);

    }
}
