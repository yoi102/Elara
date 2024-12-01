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
    internal class ProfileConfig : IEntityTypeConfiguration<Profile>
    {

        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.ToTable("T_Profiles");

        }

    }
}
