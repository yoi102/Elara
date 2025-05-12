using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Elara.ViewModel.Interfaces;
using Frontend.Shared.Identifiers;
using InteractionServices.Abstractions;
using Services.Abstractions;
using Services.Abstractions.Results.Data;
using System.Collections.ObjectModel;

namespace Elara.ViewModel.Chat;

public partial class ChatShellViewModel : ObservableObject, IHasNotificationNumber
{
    public ChatShellViewModel(IConversationQueryService conversationQueryService, IDialogService dialogService)
    {
        this.conversationQueryService = conversationQueryService;
        this.dialogService = dialogService;
    }

    public async Task InitializeAsync()
    {
        var conversationsDataResult = await conversationQueryService.GetConversationsWithMessagesAsync();
        if (!conversationsDataResult.IsSuccessful)
        {
            await dialogService.ShowMessageDialogAsync(conversationsDataResult.ErrorMessage, DialogHostIdentifiers.MainWindow);
            return;
        }
        foreach (var data in conversationsDataResult.ResultData)
        {
            ConversationModel conversationModel = DataToModel(data);

            Conversations.Add(conversationModel);
        }
    }

    #region DataToModel

    private ConversationModel DataToModel(ConversationData data)
    {
        var conversationModel = new ConversationModel()
        {
            Id = data.Id,
            IsGroup = data.IsGroup,
            CreatedAt = data.CreatedAt,
            UpdatedAt = data.UpdatedAt
        };

        foreach (var message in data.Messages)
        {
            MessageModel messageModel = DataToModel(message);

            conversationModel.Messages.Add(messageModel);
        }

        return conversationModel;
    }

    private MessageModel DataToModel(MessageData message)
    {
        var messageModel = new MessageModel(message.Sender)
        {
            IsUnread = message.IsUnread,
            Attachments = new(message.Attachments),
            Content = message.Content,
            QuoteMessage = message.QuoteMessage,
            SendAt = message.CreatedAt,
            UpdatedAt = message.UpdatedAt
        };

        foreach (var replyMessageData in message.ReplyMessages)
        {
            var replyMessageModel = DataToModel(replyMessageData);
            messageModel.ReplyMessages.Add(replyMessageModel);
        }

        return messageModel;
    }

    private ReplyMessageModel DataToModel(ReplyMessageData replyMessageData)
    {
        var replyMessageModel = new ReplyMessageModel(replyMessageData.Sender)
        {
            IsUnread = replyMessageData.IsUnread,
            Content = replyMessageData.Content,
            UpdatedAt = replyMessageData.UpdatedAt,
            MessageId = replyMessageData.MessageId,
            QuoteMessage = replyMessageData.QuoteMessage,
            CreatedAt = replyMessageData.CreatedAt
        };
        return replyMessageModel;
    }

    #endregion DataToModel

    [ObservableProperty]
    private ObservableCollection<ConversationModel> conversations = [];

    [ObservableProperty]
    private ConversationModel? selectedConversation;

    [ObservableProperty]
    private ChatViewModel? chatViewModel;

    private readonly IConversationQueryService conversationQueryService;
    private readonly IDialogService dialogService;

    public int? NotificationNumber
    {
        get
        {
            var unreadCount = Conversations.SelectMany(c => c.Messages)
                                              .Count(m => m.IsUnread);
            if (unreadCount == 0)
                return null;

            return unreadCount;
        }
    }

    [RelayCommand]
    private void CreateGroupConversation()
    {
    }
}
