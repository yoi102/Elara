using CommunityToolkit.Mvvm.ComponentModel;
using Elara.ViewModel.Chat;
using Elara.ViewModel.Interfaces;
using System.Collections.ObjectModel;

namespace Elara.ViewModel.Contact;

public partial class ContactInfoViewModel : ObservableValidator, IHasNotificationNumber
{
    public int? NotificationNumber => throw new NotImplementedException();

    [ObservableProperty]
    private ObservableCollection<ContactRequestModel> contactRequests = [];

    [ObservableProperty]
    private ObservableCollection<GroupConversationRequestModel> groupConversationRequest = [];





    private async Task SendContactRequestAsync()
    {
        await Task.CompletedTask;
    }




}
