using DomainCommons;
using DomainCommons.EntityStronglyIds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkspaceService.Domain.Entities
{
    public class Workspace : AggregateRootEntity<WorkspaceId>
    {
        public Workspace(string name)
        {
            Id = WorkspaceId.New();
            Name = name;
        }

        //public ICollection<WorkspaceConversationId> ConversationIds { get; } = new HashSet<WorkspaceConversationId>();
        public override WorkspaceId Id { get; protected set; }
        public ICollection<WorkspaceMemberId> MemberIds { get; } = new HashSet<WorkspaceMemberId>();
        public string Name { get; set; }

    }
}
