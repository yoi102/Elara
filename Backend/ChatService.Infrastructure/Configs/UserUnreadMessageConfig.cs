using ChatService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatService.Infrastructure.Configs;

internal class UserUnreadMessageConfig : IEntityTypeConfiguration<UserUnreadMessage>
{
    public void Configure(EntityTypeBuilder<UserUnreadMessage> builder)
    {
        builder.ToTable("T_UserUnreadMessages");
        builder.HasKey(e => e.Id);

        builder.HasOne<Message>()
               .WithMany()
               .HasForeignKey(e => e.MessageId)
               .OnDelete(DeleteBehavior.Cascade);// 联级删除
    }
}
