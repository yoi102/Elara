using ApiClients.Abstractions.ChatApiClient.Conversation.Responses;
using Services.Abstractions.ChatServices;
using Services.Abstractions.Results;
using Services.Abstractions.Results.ChatServices;

namespace Services.Combination;

public class ConversationService
{
    private readonly IChatMessageService chatMessageService;

    public ConversationService(IChatMessageService chatMessageService)
    {
        this.chatMessageService = chatMessageService;
    }

    public void GetConversationWithMessagesAsync()
    {



    }

    public void GetConversationUnreadMessagesAsync()
    {



    }



    public void GetConversationMessagesAsync()
    {



    }

    public async Task<ApiServiceResult<ReplyMessageData>> GetMessageReplyMessagesAsync(Guid messageId)
    {
        var replyMessagesResult = await chatMessageService.GetReplyMessagesAsync(messageId);
        if (!replyMessagesResult.IsSuccessful)
            return ApiServiceResult<ReplyMessageData>.FromFailure(replyMessagesResult);

        MessageData[] replyMessageData = replyMessagesResult.ResultData;








        return null!;

    }
}

public record ConversationResult
{
}

public record MessageResult
{
}
