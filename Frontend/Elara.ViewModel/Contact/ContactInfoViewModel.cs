using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Elara.ViewModel.Interfaces;
using System.Collections.ObjectModel;

namespace Elara.ViewModel.Contact;

public partial class ContactViewModel : ObservableObject, IHasNotificationNumber
{
    public int? NotificationNumber => throw new NotImplementedException();

    [ObservableProperty]
    private ObservableCollection<ContactRequestViewModel> contactRequests = [];

    [ObservableProperty]
    private ObservableCollection<GroupConversationRequestViewModel> groupConversationRequest = [];




    [RelayCommand]
    private async Task SendContactRequestAsync()
    {
        await Task.CompletedTask;
    }




}
