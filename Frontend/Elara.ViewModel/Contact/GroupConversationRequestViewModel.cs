using ApiClients.Abstractions.Models.Responses;
using CommunityToolkit.Mvvm.ComponentModel;
using Services.Abstractions.ChatServices;
using System.Collections.ObjectModel;

namespace Elara.ViewModel.Contact;

public partial class GroupConversationRequestViewModel : ObservableObject
{
    public GroupConversationRequestViewModel(IChatConversationRequestService chatConversationRequestService)
    {
        this.chatConversationRequestService = chatConversationRequestService;
    }

    public async Task InitializeAsync()
    {
        var conversationRequestsData = await chatConversationRequestService.GetConversationRequestsAsync();
        ContactRequestsData = new ObservableCollection<ConversationRequestData>(conversationRequestsData);
    }

    [ObservableProperty]
    private ObservableCollection<ConversationRequestData> contactRequestsData = [];

    private readonly IChatConversationRequestService chatConversationRequestService;

    public async Task AcceptAsync(ConversationRequestData groupConversationRequestData)
    {
        await chatConversationRequestService.AcceptConversationRequestAsync(groupConversationRequestData.Id);
    }

    public async Task RejectAsync(ConversationRequestData groupConversationRequestData)
    {
        await chatConversationRequestService.RejectConversationRequestAsync(groupConversationRequestData.Id);
    }
}
