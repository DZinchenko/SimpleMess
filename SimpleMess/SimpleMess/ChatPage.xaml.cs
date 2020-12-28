using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SimpleMess.Data.Entities;
using SimpleMess.Data.InternalRepositories;
using SimpleMess.Domain.Interfaces;
using SimpleMess.ChatShortcutBuildStrategy;

namespace SimpleMess
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : ContentPage
    {
        private IInternalMessageRepo _msgRepo;
        private IInternalUserRepo _userRepo;
        private IMessageService _msgServ;
        private IAuthorizationManager _authManager;
        private IChatManager _chatManager;
        private IBuildChatShortcutStrategyFactory _buildChatShortcutStrategyFactory;

        private Message _lastMsg;

        public ChatPage(IInternalMessageRepo msgRepo,
                        IInternalUserRepo userRepo,
                        IMessageService msgServ,
                        IAuthorizationManager authManager,
                        IChatManager chatManager,
                        IBuildChatShortcutStrategyFactory buildChatShortcutStrategyFactory)
        {
            _msgRepo = msgRepo;
            _userRepo = userRepo;
            _msgServ = msgServ;
            _authManager = authManager;
            _chatManager = chatManager;
            _buildChatShortcutStrategyFactory = buildChatShortcutStrategyFactory;

            InitializeComponent();
            ShowMessages(_msgRepo.GetMessagesInChat(_chatManager.GetActiveChat().Id));

            var chatShortcut = _buildChatShortcutStrategyFactory.Create(_chatManager.GetActiveChat()).ExtractChatShortcut(_chatManager.GetActiveChat());
            ChatNameLabel.Text = chatShortcut.Name;
            ChatImage.Source = chatShortcut.ImageSource;

            Device.StartTimer(TimeSpan.FromSeconds(0.5), ()=>
                {
                    Device.BeginInvokeOnMainThread(
                        () => ShowMessages(_msgRepo.GetNewMessagesInChat(_chatManager.GetActiveChat().Id, _lastMsg))
                        );
                    return Navigation.NavigationStack.Contains(this);
                });
        }

        private void ShowMessages(List<Message> messages)
        {
            if (messages.Count > 0)
            {
                foreach (var message in messages)
                {
                    Frame msgFrame;

                    if (message.UserId == _authManager.GetAuthorizedUser().Id)
                    {
                        msgFrame = new Frame
                        {
                            BackgroundColor = Color.LightSkyBlue,
                            WidthRequest = 300,
                            HorizontalOptions = LayoutOptions.End,
                            CornerRadius = 5,
                            Padding = 10,
                            Content = new StackLayout
                            {
                                Children =
                    {
                        new Label
                        {
                            HorizontalOptions = LayoutOptions.Start,
                            VerticalOptions = LayoutOptions.Start,
                            TextColor = Color.Black,
                            FontSize = 18,
                        },
                        new Label
                        {
                            HorizontalOptions = LayoutOptions.End,
                            VerticalOptions = LayoutOptions.End,
                            TextColor = Color.DarkSlateBlue,
                        }
                    }
                            }
                        };
                    }
                    else
                    {
                        msgFrame = new Frame
                        {
                            BackgroundColor = Color.DodgerBlue,
                            WidthRequest = 300,
                            HorizontalOptions = LayoutOptions.Start,
                            CornerRadius = 5,
                            Padding = 10,
                            Content = new StackLayout
                            {
                                Children =
                    {
                        new Label
                        {
                            HorizontalOptions = LayoutOptions.Start,
                            VerticalOptions = LayoutOptions.Start,
                            TextColor = Color.Black,
                            FontSize = 18,
                        },
                        new Label
                        {
                            HorizontalOptions = LayoutOptions.End,
                            VerticalOptions = LayoutOptions.End,
                            TextColor = Color.DarkSlateBlue,
                        }
                    }
                            }
                        };
                        if (_lastMsg == null || _lastMsg.UserId != message.UserId)
                        {
                            ((StackLayout)(msgFrame.Content)).Children.Insert(0, new Label
                            {
                                Text = _userRepo.GetUserById(message.UserId).Username,
                                FontAttributes = FontAttributes.Bold,
                                FontSize = 18,
                                TextColor = Color.MidnightBlue,
                            });
                        }
                    }

                    MessagesView.Children.Add(msgFrame);
                    _lastMsg = message;
                }

                _msgServ.UserSeenMessages(messages, _authManager.GetAuthorizedUser());
            }
        }

        private void SendBtn_Clicked(object sender, EventArgs e)
        {
            var newMessage = new Message
            {
                ChatId = _chatManager.GetActiveChat().Id,
                Text = MessageEntry.Text,
                UserId = _authManager.GetAuthorizedUser().Id,
                Time = DateTime.Now,
            };

            _msgServ.UserSeenMessages(new List<Message> { newMessage }, _authManager.GetAuthorizedUser());

            ShowMessages(new List<Message> { newMessage });
        }

        private void BackBtn_Clicked(object sender, EventArgs e)
        {
            _chatManager.DeactivateChat();
            Navigation.PopModalAsync();
        }
    }
}