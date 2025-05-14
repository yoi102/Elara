using Services.Abstractions.ChatServices;
using Services.Abstractions.Results.Data;

namespace Services.Combination;
public class GroupConversationRequest
{
    private readonly IChatConversationRequestService chatConversationRequestService;

    public GroupConversationRequest(IChatConversationRequestService chatConversationRequestService)
    {
        this.chatConversationRequestService = chatConversationRequestService;
    }

    public async Task GetCurrentUserConversationRequest()
    {
        var conversationRequestResult = await chatConversationRequestService.GetConversationRequestsAsync();

        if (!conversationRequestResult.IsSuccessful)
            return;////////

        foreach (var data in conversationRequestResult.ResultData)
        {
            //data.SenderId
            //data.ReceiverId
            //data.ConversationId





        }
    }

}

record GroupConversationInfoData
{

}
record GroupConversationRequestData
{
    public required UserInfoData Sender { get; init; }
    public required UserInfoData Receiver { get; init; }
    public required string Role { get; init; }
    public required GroupConversationInfoData GroupConversationInfo { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required DateTimeOffset? UpdatedAt { get; init; }
}








