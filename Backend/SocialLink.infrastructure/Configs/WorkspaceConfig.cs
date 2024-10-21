using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialLink.Domain.Entities;

namespace SocialLink.infrastructure.Configs
{
    internal class WorkspaceConfig : IEntityTypeConfiguration<Workspace>
    {
        public void Configure(EntityTypeBuilder<Workspace> builder)
        {
            builder.ToTable("T_Workspaces");
        }
    }
}
