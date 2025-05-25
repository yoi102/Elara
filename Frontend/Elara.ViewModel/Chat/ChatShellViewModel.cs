using ApiClients.Abstractions.Models.Responses;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Elara.ViewModel.Interfaces;
using Frontend.Shared.Identifiers;
using InteractionServices.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions.ChatServices;
using System.Collections.ObjectModel;

namespace Elara.ViewModel.Chat;

public partial class ChatShellViewModel : ObservableObject, IHasNotificationNumber
{
    public ChatShellViewModel(IServiceProvider serviceProvider, IChatConversationService chatConversationService, IDialogService dialogService)
    {
        this.serviceProvider = serviceProvider;
        this.chatConversationService = chatConversationService;
        this.dialogService = dialogService;
    }

    public async Task InitializeAsync()
    {
        var conversationsDataResult = await chatConversationService.GetUserConversationsAsync();

        foreach (var data in conversationsDataResult)
        {
            ConversationViewModel conversationModel = DataToModel(data);

            Conversations.Add(conversationModel);
        }
    }

    #region DataToModel 应该弄到工厂上？ 晚点吧

    private ConversationViewModel DataToModel(ConversationDetailsData data)
    {
        var conversationModel = serviceProvider.GetService<ConversationViewModel>()!;
        conversationModel.ConversationData = data;

        foreach (var message in data.Messages)
        {
            MessageViewModel messageModel = DataToModel(message);
            conversationModel.Messages.Add(messageModel);
        }

        return conversationModel;
    }

    private MessageViewModel DataToModel(MessageWithReplyMessageData message)
    {
        var messageModel = serviceProvider.GetService<MessageViewModel>()!;
        messageModel.MessageData = message;

        foreach (var replyMessageData in message.ReplyMessages)
        {
            var replyMessageModel = DataToModel(replyMessageData);
            messageModel.ReplyMessages.Add(replyMessageModel);
        }

        return messageModel;
    }

    private ReplyMessageViewModel DataToModel(ReplyMessageData replyMessageData)
    {
        var replyMessageModel = serviceProvider.GetService<ReplyMessageViewModel>()!;
        replyMessageModel.ReplyMessageData = replyMessageData;

        return replyMessageModel;
    }

    #endregion DataToModel 应该弄到工厂上？ 晚点吧

    [ObservableProperty]
    private ObservableCollection<ConversationViewModel> conversations = [];

    [ObservableProperty]
    private ConversationViewModel? selectedConversation;

    private readonly IServiceProvider serviceProvider;
    private readonly IChatConversationService chatConversationService;
    private readonly IDialogService dialogService;

    public int? NotificationNumber
    {
        get
        {
            var unreadCount = Conversations.SelectMany(c => c.Messages)
                                              .Count(m => m.MessageData!.IsUnread);
            if (unreadCount == 0)
                return null;

            return unreadCount;
        }
    }

    [RelayCommand]
    private void CreateGroupConversationAsync()
    {
    }
}
