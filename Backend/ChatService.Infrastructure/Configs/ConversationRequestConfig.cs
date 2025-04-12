using ChatService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatService.Infrastructure.Configs;

internal class ConversationRequestConfig : IEntityTypeConfiguration<ConversationRequest>
{
    public void Configure(EntityTypeBuilder<ConversationRequest> builder)
    {
        builder.ToTable("T_ConversationRequests");
        builder.HasKey(e => e.Id);
    }
}
