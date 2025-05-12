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

    public UploadedItemId? AvatarItemId { get; private set; }
    public string DisplayName { get; private set; } = null!;
    public override ProfileId Id { get; protected set; }
    public UserId UserId { get; private set; }

    public void ChangeDisplayName(string value)
    {
        if (DisplayName == value) return;
        DisplayName = value;
        this.AddDomainEventIfAbsent(new ProfileUpdatedEvent(this));
        NotifyModified();
    }

    public void ChangeAvatar(UploadedItemId value)
    {
        if (AvatarItemId == value) return;
        AvatarItemId = value;
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
