using Commons.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions;
using Services.Abstractions.ChatServices;
using Services.Abstractions.PersonalSpaceServices;
using Services.ChatServices;
using Services.Combination;
using Services.PersonalSpaceServices;

namespace Services;

public class FrontendModuleInitializer : IFrontendModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddTransient<IChatConversationRequestService, ChatConversationRequestService>();
        services.AddTransient<IChatConversationRequestService, ChatConversationRequestService>();
        services.AddTransient<IChatMessageService, ChatMessageService>();
        services.AddTransient<IChatParticipantService, ChatParticipantService>();
        services.AddTransient<IFileService, FileService>();
        services.AddTransient<IPersonalSpaceContactRequestService, PersonalSpaceContactRequestService>();
        services.AddTransient<IPersonalSpaceContactService, PersonalSpaceContactService>();
        services.AddTransient<IPersonalSpaceProfileService, PersonalSpaceProfileService>();
        services.AddTransient<IUserIdentityService, UserIdentityService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IConversationQueryService, ConversationQueryService>();
    }
}
