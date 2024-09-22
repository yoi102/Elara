using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalSpaceService.Domain.Entities;

namespace PersonalSpaceService.Infrastructure.Configs;

internal class ProfileConfig : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.UserId)
               .IsRequired();

        builder.ToTable("T_Profiles");
    }
}
