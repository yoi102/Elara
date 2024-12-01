using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalSpaceService.Domain.Entities;

namespace PersonalSpaceService.Infrastructure.Configs
{
    internal class ContactConfig : IEntityTypeConfiguration<Contact>
    {

        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("T_Contacts");

        }

    }
}
