using Strongly;

[assembly: StronglyDefaults(converters: StronglyConverter.EfValueConverter |
             StronglyConverter.SystemTextJson | 
             StronglyConverter.TypeConverter)]

namespace DomainCommons.EntityStronglyIds;

[Strongly]
public partial struct ContactId;

[Strongly]
public partial struct ContactRequestId;

[Strongly]
public partial struct ConversationId;

[Strongly]
public partial struct ConversationRequestId;

[Strongly]
public partial struct ParticipantId;

[Strongly]
public partial struct MessageId;

[Strongly]
public partial struct ReplyMessageId;

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

[Strongly]
public partial struct UserUnreadMessageId;
[Strongly]
public partial struct UserUnreadConversationRequestId;

[Strongly]
public partial struct MessageAttachmentId;

// Add more strongly typed IDs as needed
