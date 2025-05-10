using CommunityToolkit.Mvvm.ComponentModel;

namespace Elara.ViewModel.Chat;

public partial class ParticipantModel : ObservableObject
{
    [ObservableProperty]
    private Uri? avatarUrl;

    [ObservableProperty]
    private string? name;

    [ObservableProperty]
    private string? role;

    public Guid ParticipantId { get; init; }
    public Guid UserId { get; init; }
}
