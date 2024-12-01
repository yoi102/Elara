using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PersonalSpaceService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalSpaceService.Infrastructure.Configs
{
    internal class SocialContextConfig : IEntityTypeConfiguration<SocialContext>
    {

        public void Configure(EntityTypeBuilder<SocialContext> builder)
        {
            builder.ToTable("T_SocialContexts");

        }

    }
}
