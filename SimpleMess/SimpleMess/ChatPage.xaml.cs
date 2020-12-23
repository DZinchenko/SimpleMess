using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SimpleMess.Data.Entities;
using SimpleMess.Data.Repositories;
using SimpleMess.Domain.Interfaces;

namespace SimpleMess
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : ContentPage
    {
        private IChatRepository _chatRepo;
        private IMessageRepository _msgRepo;
        private IUserRepository _userRepo;
        private IChatService _chatServ;
        private IMessageService _msgServ;

        private Message _lastMsg;

        public ChatPage(IChatRepository chatRepo,
                        IMessageRepository msgRepo,
                        IUserRepository userRepo,
                        IChatService chatServ,
                        IMessageService msgServ)
        {
            _chatRepo = chatRepo;
            _msgRepo = msgRepo;
            _userRepo = userRepo;
            _chatServ = chatServ;
            _msgServ = msgServ;

            InitializeComponent();
            ShowMessages(_msgRepo.GetMessagesInChat(App.CurrentChat.Id));

            Device.StartTimer(TimeSpan.FromSeconds(0.5), ()=>
                {
                    Device.BeginInvokeOnMainThread(
                        () => ShowMessages(_msgRepo.GetNewMessagesInChat(App.CurrentChat.Id, _lastMsg))
                        );
                    return this.IsFocused;
                });
        }

        private void ShowMessages(List<Message> messages)
        {
            if (messages.Count > 0)
            {
                foreach (var message in messages)
                {
                    Frame msgFrame;

                    if (message.UserId == App.CurrentUser.Id)
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

                _msgServ.UserSeenMessages(messages, App.CurrentUser);
            }
        }

        private void SendBtn_Clicked(object sender, EventArgs e)
        {
            var newMessage = new Message
            {
                ChatId = App.CurrentChat.Id,
                Text = MessageEntry.Text,
                UserId = App.CurrentUser.Id,
                Time = DateTime.Now,
            };

            _msgServ.UserSeenMessages(new List<Message> { newMessage }, App.CurrentUser);

            ShowMessages(new List<Message> { newMessage });
        }

        private void BackBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
            App.CurrentChat = null;
        }
    }
}