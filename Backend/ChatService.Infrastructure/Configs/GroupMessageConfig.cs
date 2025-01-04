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
    internal class GroupMessageConfig : IEntityTypeConfiguration<GroupMessage>
    {
        public void Configure(EntityTypeBuilder<GroupMessage> builder)
        {
            builder.ToTable("T_GroupMessages");
            builder.HasKey(e => e.Id);

            builder.Navigation(e => e.Attachments)
                   .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}