using ChatService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatService.Infrastructure.Configs
{
    internal class PersonalConversationConfig : IEntityTypeConfiguration<PersonalConversation>
    {
        public void Configure(EntityTypeBuilder<PersonalConversation> builder)
        {
            builder.ToTable("T_PersonalConversations");
            builder.HasKey(e => e.Id);

        }
    }
}