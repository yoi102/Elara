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

        builder.Navigation(e => e.Attachments)
               .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasOne<BaseMessage>()
               .WithMany()
               .HasForeignKey(e => e.MessageId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
