using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SimpleMess.Data.InternalRepositories;
using SimpleMess.InnerEF.Repositories;
using SimpleMess.Data.Entities;
using SimpleMess.Domain.Interfaces;
using System.IO;

namespace SimpleMess
{
    public partial class MainPage : ContentPage
    {
        private IInternalUserRepo _userRepo;
        private IInternalChatRepo _chatRepo;
        private IInternalMessageRepo _msgRepo;
        private IChatService _chatServ;

        private Dictionary<Frame, Chat> _chatFrames;

        public MainPage(IInternalUserRepo userRepo,
                        IInternalChatRepo chatRepo,
                        IInternalMessageRepo msgRepo,
                        IChatService chatService)
        {
            _userRepo = userRepo;
            _chatRepo = chatRepo;
            _msgRepo = msgRepo;
            _chatServ = chatService;

            InitializeComponent();
            _chatFrames = new Dictionary<Frame, Chat>();
            UpdatePage();
        }

        private void BackBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void Chat_Clicked(object sender)
        {
            App.CurrentChat = _chatFrames[(Frame)sender];
            Navigation.PushAsync(DependencyService.Resolve<ChatPage>());
        }

        private void UpdatePage()
        {
            ChatsView.Children.Clear();
            foreach (var chat in _chatRepo.GetAllChatsForUser(App.CurrentUser.Id).OrderByDescending(chat => _msgRepo.GetMessagesInChat(chat.Id).Max(msg=>msg.Time)))
            {
                ShowChatFrame(chat);
            }
        }

        private void ShowChatFrame(Chat chat)
        {
            var messages = _msgRepo.GetMessagesInChat(chat.Id);
            ImageSource chatImageSource;
            string chatName;
            string lastMsgText = "";
            string unseenMsgNumStr = "0";

            if (chat.GetType() == typeof(GroupChat))
            {
                var groupChat = (GroupChat)chat;
                if (groupChat.Picture != null)
                {
                    chatImageSource = ImageSource.FromStream(() => new MemoryStream(groupChat.Picture));
                }
                else
                {
                    chatImageSource = null;
                }

                chatName = groupChat.Name;
            }
            else
            {
                var secondUser = _userRepo.GetUserById(chat.UserChats.Select(uc => uc.UserId).Where(id => id != App.CurrentUser.Id).First());
                if (secondUser.Photo != null)
                {
                    chatImageSource = ImageSource.FromStream(() => new MemoryStream(secondUser.Photo));
                }
                else
                {
                    chatImageSource = null;
                }
                chatName = secondUser.Username;
            }

            if (_msgRepo.GetMessagesInChat(chat.Id).Count > 0)
            {
                var lastMsg = messages.Find(msg => msg.Time == messages.Max(m => m.Time));
                var lastMsgUser = _userRepo.GetUserById(lastMsg.UserId);
                lastMsgText = lastMsgUser.Username + ": " + lastMsg.Text.Substring(0, 25 - lastMsgUser.Username.Length - 2);

                var unseenMsgNum = messages.Count((msg) =>
                    {
                        if (!msg.UserSeenMessages.Select(usm => usm.UserId).Contains(App.CurrentUser.Id)) { return true; }
                        else { return false; }
                    });
                unseenMsgNumStr = unseenMsgNum.ToString();
            }

            var newChatFrame = new Frame
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Start,
                HeightRequest = 80,
                BorderColor = Color.Black,
                Padding = 1,
                Content = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Fill,
                    BackgroundColor = Color.WhiteSmoke,
                    Padding = 10,
                    Children =
                    {
                        new Frame
                        {
                            HeightRequest = 60,
                            WidthRequest = 60,
                            HorizontalOptions = LayoutOptions.Start,
                            VerticalOptions = LayoutOptions.Center,
                            Padding = 0,
                            CornerRadius = 20,
                            IsClippedToBounds = true,
                            Content = new Image
                            {
                                HeightRequest = 60,
                                WidthRequest = 60,
                                Aspect = Aspect.AspectFill,
                                Source = chatImageSource
                            }
                        },
                        new StackLayout
                        {
                            Spacing = 5,
                            Children =
                            {
                                new Label
                                {
                                    Text = chatName,
                                    FontSize = 18,
                                    TextColor=Color.Black
                                },
                                new Label
                                {
                                    Text = lastMsgText,
                                    FontSize = 17,
                                    TextColor = Color.Gray
                                }
                            }
                        },
                        new Frame
                        {
                            HeightRequest = 30,
                            WidthRequest = 30,
                            HorizontalOptions = LayoutOptions.EndAndExpand,
                            VerticalOptions = LayoutOptions.Center,
                            Padding = 0,
                            CornerRadius = 10,
                            BackgroundColor = Color.IndianRed,
                            Content = new Label
                            {
                                TextColor = Color.White,
                                HorizontalOptions = LayoutOptions.Fill,
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalTextAlignment = TextAlignment.Center,
                                FontSize = 14,
                                Text = unseenMsgNumStr,
                            }
                        }
                    }
                }
            };

            newChatFrame.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(Chat_Clicked)});

            ChatsView.Children.Add(newChatFrame);
            _chatFrames.Add(newChatFrame, chat);
        }
    }
}
