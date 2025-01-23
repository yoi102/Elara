using DomainCommons;
using DomainCommons.EntityStronglyIds;

namespace WorkspaceService.Domain.Entities;

public record WorkspaceMember : Entity<WorkspaceMemberId>
{
    public WorkspaceMember(UserId userId, string role)
    {
        Id = WorkspaceMemberId.New();
        UserId = userId;
        Role = role;
    }

    private WorkspaceMember()
    {
    }
    public override WorkspaceMemberId Id { get; protected set; }
    public UserId UserId { get; private set; }
    public string Role { get; private set; } = null!;

}
