using ChatService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatService.Infrastructure.Configs
{
    internal class ReplyMessageConfig : IEntityTypeConfiguration<PersonalMessage>
    {
        public void Configure(EntityTypeBuilder<PersonalMessage> builder)
        {
            builder.ToTable("T_ReplyMessages");
            builder.HasKey(e => e.Id);

            builder.Navigation(e => e.Attachments)
                   .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}