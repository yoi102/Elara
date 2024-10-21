using DomainCommons;
using Strongly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLink.Domain.Entities
{
    [Strongly(converters: StronglyConverter.EfValueConverter |
                          StronglyConverter.SwaggerSchemaFilter |
                          StronglyConverter.SystemTextJson |
                          StronglyConverter.TypeConverter)]
    public partial struct WorkspaceId;

    public class Workspace : AggregateRootEntity<WorkspaceId>
    {
        public Workspace()
        {
            Id = WorkspaceId.New();

            MemberId = new HashSet<UserId>();
        }

        public override WorkspaceId Id { get; }


        public ICollection<UserId> MemberId { get; }






    }
}
