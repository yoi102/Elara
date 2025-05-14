using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Elara.ViewModel.Interfaces;

namespace Elara.ViewModel.Contact;

public partial class RequestViewModel : ObservableObject, IHasNotificationNumber
{
    public int? NotificationNumber => throw new NotImplementedException();

    [ObservableProperty]
    private ContactRequestViewModel? contactRequestViewModel;

    [ObservableProperty]
    private GroupConversationRequestViewModel? groupConversationRequestViewModel;




    [RelayCommand]
    private async Task SendContactRequestAsync()
    {
        await Task.CompletedTask;
    }




}
