using Strongly;

[assembly: StronglyDefaults(converters: StronglyConverter.EfValueConverter |
             StronglyConverter.SwaggerSchemaFilter |
             StronglyConverter.SystemTextJson |
             StronglyConverter.TypeConverter)]

namespace DomainCommons.EntityStronglyIds
{
    [Strongly]
    public partial struct UploadedItemId;

    [Strongly]
    public partial struct UserId;

    [Strongly]
    public partial struct ProfileId;

    [Strongly]
    public partial struct SocialContextId;

    [Strongly]
    public partial struct ContactId;

    [Strongly]
    public partial struct WorkspaceId;   
    [Strongly]
    public partial struct WorkspaceMemberId;   
    [Strongly]
    public partial struct ConversationId;   
    [Strongly]
    public partial struct GroupConversationId;
        [Strongly]
    public partial struct MessageId;






    // Add more strongly typed IDs as needed




}