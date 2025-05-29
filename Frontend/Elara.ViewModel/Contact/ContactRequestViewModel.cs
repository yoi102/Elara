using ApiClients.Abstractions.Models.Responses;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Services.Abstractions.PersonalSpaceServices;
using System.Collections.ObjectModel;

namespace Elara.ViewModel.Contact;

public partial class ContactRequestViewModel : ObservableObject
{
    public ContactRequestViewModel(IPersonalSpaceContactRequestService personalSpaceContactRequestService)
    {
        this.personalSpaceContactRequestService = personalSpaceContactRequestService;
    }

    public async Task InitializeAsync()
    {
        var contactRequestsData = await personalSpaceContactRequestService.GetReceivedContactRequestsAsync();
        ContactRequestsData = new ObservableCollection<ContactRequestData>(contactRequestsData);
    }

    [ObservableProperty]
    private ObservableCollection<ContactRequestData> contactRequestsData = [];

    private readonly IPersonalSpaceContactRequestService personalSpaceContactRequestService;

    [RelayCommand]
    public async Task AcceptAsync(ContactRequestData contactRequestData)
    {
        await personalSpaceContactRequestService.AcceptContactRequestAsync(contactRequestData.Id);
    }

    [RelayCommand]
    public async Task RejectAsync(ContactRequestData contactRequestData)
    {
        await personalSpaceContactRequestService.RejectContactRequestAsync(contactRequestData.Id);
    }
}
