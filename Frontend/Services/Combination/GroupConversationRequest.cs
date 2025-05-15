using Services.Abstractions;
using Services.Abstractions.ChatServices;
using Services.Abstractions.Results;
using Services.Abstractions.Results.Data;

namespace Services.Combination;

public class GroupConversationRequest : IGroupConversationRequest
{
    private readonly IChatConversationRequestService chatConversationRequestService;
    private readonly IUserProfileService userProfileService;
    private readonly IChatConversationService conversationService;

    public GroupConversationRequest(IChatConversationRequestService chatConversationRequestService,
        IUserProfileService userProfileService,
        IChatConversationService conversationService)
    {
        this.chatConversationRequestService = chatConversationRequestService;
        this.userProfileService = userProfileService;
        this.conversationService = conversationService;
    }

    public async Task<ApiServiceResult<GroupConversationRequestData[]>> GetCurrentUserConversationRequests()
    {
        var conversationRequestResult = await chatConversationRequestService.GetConversationRequestsAsync();

        if (!conversationRequestResult.IsSuccessful)
            return ApiServiceResult<GroupConversationRequestData[]>.FromFailure(conversationRequestResult);
        var conversationRequests = new List<GroupConversationRequestData>();
        foreach (var data in conversationRequestResult.ResultData)
        {
            var senderInfoResult = await userProfileService.GetUserInfoDataById(data.SenderId);

            if (!senderInfoResult.IsSuccessful)
                return ApiServiceResult<GroupConversationRequestData[]>.FromFailure(senderInfoResult);

            var conversationDataResult = await conversationService.FindByIdAsync(data.ConversationId);
            if (!conversationDataResult.IsSuccessful)
                return ApiServiceResult<GroupConversationRequestData[]>.FromFailure(conversationDataResult);

            var conversationRequest = new GroupConversationRequestData()
            {
                Id = data.Id,
                Sender = senderInfoResult.ResultData,
                ConversationName = conversationDataResult.ResultData.Name,
                Role = data.Role,
                CreatedAt = data.CreatedAt,
                UpdatedAt = data.UpdatedAt,
            };
            conversationRequests.Add(conversationRequest);
        }

        return new ApiServiceResult<GroupConversationRequestData[]>()
        {
            IsSuccessful = true,
            IsServerError = false,
            ResultData = conversationRequests.ToArray(),
        };
    }
}
