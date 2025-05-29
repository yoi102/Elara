using ApiClients.Abstractions.Models.Responses;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Elara.ViewModel.Interfaces;
using Services.Abstractions.PersonalSpaceServices;
using System.Collections.ObjectModel;

namespace Elara.ViewModel.Contact;

public partial class ContactShellViewModel : ObservableObject, IHasNotificationNumber
{
    public ContactShellViewModel(IPersonalSpaceContactService personalSpaceContactService)
    {
        this.personalSpaceContactService = personalSpaceContactService;
    }

    [ObservableProperty]
    public RequestViewModel? requestViewModel;

    private readonly IPersonalSpaceContactService personalSpaceContactService;

    public int? NotificationNumber => throw new NotImplementedException();

    [ObservableProperty]
    private ObservableCollection<ContactData> contactsData = [];

    [ObservableProperty]
    private ContactData? selectedContact;

    public async Task InitializeAsync()
    {
        var contactsData = await personalSpaceContactService.GetContactsAsync();

        ContactsData = new ObservableCollection<ContactData>(contactsData);
    }

    [RelayCommand]
    private void Chat()
    {
    }

    [RelayCommand]
    private async Task UpdateMarkerAsync(string remark)
    {
        if (SelectedContact is null)
            return;

        await personalSpaceContactService.UpdateContactInfoAsync(SelectedContact.Id, new() { Remark = remark });
    }
}
