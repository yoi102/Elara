using ChatService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatService.Infrastructure.Configs;

internal class MessageAttachmentConfig : IEntityTypeConfiguration<MessageAttachment>
{
    public void Configure(EntityTypeBuilder<MessageAttachment> builder)
    {
        builder.ToTable("T_MessageAttachments");
        builder.HasKey(e => e.Id);

        builder.HasOne<Message>()
               .WithMany()
               .HasForeignKey(e => e.MessageId)
               .OnDelete(DeleteBehavior.Cascade);// 联级删除
    }
}
