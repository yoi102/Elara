using CommunityToolkit.Mvvm.ComponentModel;
using Elara.ViewModel.Interfaces;

namespace Elara.ViewModel.Contact;

public partial class ContactShellViewModel : ObservableObject, IHasNotificationNumber
{
    [ObservableProperty]
    public RequestViewModel? requestViewModel;

    public int? NotificationNumber => throw new NotImplementedException();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        




    public async Task InitializeAsync()
    {


    }






}
