using Strongly;

[assembly: StronglyDefaults(converters: StronglyConverter.EfValueConverter |
             StronglyConverter.SwaggerSchemaFilter |
             StronglyConverter.SystemTextJson |
             StronglyConverter.TypeConverter)]

namespace DomainCommons.EntityStronglyIds
{
    [Strongly]
    public partial struct ContactId;

    [Strongly]
    public partial struct GroupConversationId;

    [Strongly]
    public partial struct GroupConversationMemberId;

    [Strongly]
    public partial struct MessageId;

    [Strongly]
    public partial struct PersonalConversationId;
    [Strongly]
    public partial struct ProfileId;

    [Strongly]
    public partial struct UploadedItemId;

    [Strongly]
    public partial struct UserId;

    [Strongly]
    public partial struct WorkspaceId;

    [Strongly]
    public partial struct WorkspaceMemberId;

    // Add more strongly typed IDs as needed
}
