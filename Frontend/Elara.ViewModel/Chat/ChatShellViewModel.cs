using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Elara.ViewModel.Interfaces;
using Frontend.Shared.Identifiers;
using InteractionServices.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions;
using Services.Abstractions.ChatServices;
using Services.Abstractions.Results.Data;
using System.Collections.ObjectModel;

namespace Elara.ViewModel.Chat;

public partial class ChatShellViewModel : ObservableObject, IHasNotificationNumber
{
    public ChatShellViewModel(IServiceProvider serviceProvider, IChatConversationRequestService chatConversationRequestService, IDialogService dialogService)
    {
        this.serviceProvider = serviceProvider;
        this.chatConversationRequestService = chatConversationRequestService;
        this.dialogService = dialogService;
    }

    public async Task InitializeAsync()
    {
        //var conversationsDataResult = await chatConversationRequestService.GetConversationsWithMessagesAsync();
        //if (!conversationsDataResult.IsSuccessful)
        //{
        //    await dialogService.ShowMessageDialogAsync(conversationsDataResult.ErrorMessage, DialogHostIdentifiers.MainWindow);
        //    return;
        //}
        //foreach (var data in conversationsDataResult.ResultData)
        //{
        //    ConversationViewModel conversationModel = DataToModel(data);

        //    Conversations.Add(conversationModel);
        //}
    }

    #region DataToModel 应该弄到工厂上？ 晚点吧

    private ConversationViewModel DataToModel(ConversationData data)
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

    private MessageViewModel DataToModel(MessageData message)
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
    private readonly IChatConversationRequestService chatConversationRequestService;
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
