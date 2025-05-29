using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Elara.ViewModel.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Elara.ViewModel.Contact;

public partial class RequestViewModel : ObservableObject, IHasNotificationNumber
{
    public RequestViewModel(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public int? NotificationNumber => throw new NotImplementedException();

    [ObservableProperty]
    private ContactRequestViewModel? contactRequestViewModel;

    [ObservableProperty]
    private GroupConversationRequestViewModel? groupConversationRequestViewModel;

    private readonly IServiceProvider serviceProvider;

    public async Task InitializeAsync()
    {
        ContactRequestViewModel = serviceProvider.GetService<ContactRequestViewModel>()!;
        GroupConversationRequestViewModel = serviceProvider.GetService<GroupConversationRequestViewModel>()!;

        await ContactRequestViewModel.InitializeAsync();
        await GroupConversationRequestViewModel.InitializeAsync();
    }

    [RelayCommand]
    private async Task SendContactRequestAsync()
    {
        await Task.CompletedTask;
    }
}
