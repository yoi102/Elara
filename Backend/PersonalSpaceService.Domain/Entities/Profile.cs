using DomainCommons;
using DomainCommons.EntityStronglyIds;
using PersonalSpaceService.Domain.Events;

namespace PersonalSpaceService.Domain.Entities;

public record Profile : AggregateRootEntity<ProfileId>
{
    public Profile(UserId userId, string displayName)
    {
        UserId = userId;
        Id = ProfileId.New();
        DisplayName = displayName;
    }

    private Profile()
    {
    }

    public UploadedItemId Avatar { get; private set; }
    public string DisplayName { get; private set; } = null!;
    public override ProfileId Id { get; protected set; }
    public UserId UserId { get; private set; }

    public void ChangeDisplayName(string displayName)
    {
        if (DisplayName == displayName) return;
        DisplayName = displayName;
        this.AddDomainEventIfAbsent(new ProfileUpdatedEvent(this));
        NotifyModified();
    }

    public void ChangeAvatar(UploadedItemId avatar)
    {
        if (Avatar == avatar) return;
        Avatar = avatar;
        this.AddDomainEventIfAbsent(new ProfileUpdatedEvent(this));
        NotifyModified();
    }
    public override void SoftDelete()
    {
        if (IsDeleted) return;
        base.SoftDelete();
        this.AddDomainEventIfAbsent(new ProfileUpdatedEvent(this));
    }
}
