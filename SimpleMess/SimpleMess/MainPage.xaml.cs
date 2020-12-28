using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SimpleMess.Data.InternalRepositories;
using SimpleMess.Data.Entities;
using SimpleMess.Domain.Interfaces;
using SimpleMess.ChatShortcutBuildStrategy;

namespace SimpleMess
{
    public partial class MainPage : ContentPage
    {
        private IInternalUserRepo _userRepo;
        private IInternalMessageRepo _msgRepo;
        private IChatService _chatServ;
        private IPageFactory _pageFactory;
        private IAuthorizationManager _authManager;
        private IChatManager _chatManager;
        private IBuildChatShortcutStrategyFactory _buildChatShortcutStrategyFactory;

        private Dictionary<Frame, Chat> _chatFrames;
        private DateTime _lastUpdateTime;

        public MainPage(IInternalUserRepo userRepo,
                        IInternalMessageRepo msgRepo,
                        IChatService chatService,
                        IPageFactory pageFactory,
                        IAuthorizationManager authManager,
                        IChatManager chatManager,
                        IBuildChatShortcutStrategyFactory buildChatShortcutStrategyFactory)
        {
            _userRepo = userRepo;
            _msgRepo = msgRepo;
            _chatServ = chatService;
            _pageFactory = pageFactory;
            _authManager = authManager;
            _chatManager = chatManager;
            _buildChatShortcutStrategyFactory = buildChatShortcutStrategyFactory;

            InitializeComponent();
            
            _chatFrames = new Dictionary<Frame, Chat>();
            _lastUpdateTime = DateTime.MinValue;

            Device.StartTimer(TimeSpan.FromSeconds(0.5), () =>
            {
                Device.BeginInvokeOnMainThread(
                    () => UpdatePage()
                    );
                return Navigation.NavigationStack.Contains(this);
            });
        }

        private void BackBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void Chat_Clicked(object sender)
        {
            _chatManager.ActivateChat(_chatFrames[(Frame)sender]);
            Navigation.PushAsync(_pageFactory.CreatePage<ChatPage>());
        }

        private void UpdatePage()
        {
            foreach(var chat in _chatServ.GetChatsWithMsgAfterTime(_lastUpdateTime))
            {
                var chatFrame = _chatFrames.Keys.FirstOrDefault(frame => _chatFrames[frame] == chat);
                ChatsView.Children.Remove(chatFrame);
                _chatFrames.Remove(chatFrame);

                var newFrame = GetChatFrame(chat);
                ChatsView.Children.Add(newFrame);
                _chatFrames.Add(newFrame, chat);
            }

            _lastUpdateTime = DateTime.Now;
        }

        private Frame GetChatFrame(Chat chat)
        {
            var messages = _msgRepo.GetMessagesInChat(chat.Id);
            string lastMsgText = "";
            string unseenMsgNumStr = "0";

            var chatShortcut = _buildChatShortcutStrategyFactory.Create(chat).ExtractChatShortcut(chat);

            if (_msgRepo.GetMessagesInChat(chat.Id).Count > 0)
            {
                var lastMsg = messages.Find(msg => msg.Time == messages.Max(m => m.Time));
                var lastMsgUser = _userRepo.GetUserById(lastMsg.UserId);
                lastMsgText = lastMsgUser.Username + ": " + lastMsg.Text.Substring(0, 25 - lastMsgUser.Username.Length - 2);

                var unseenMsgNum = messages.Count((msg) =>
                    {
                        if (!msg.UserSeenMessages.Select(usm => usm.UserId).Contains(_authManager.GetAuthorizedUser().Id)) { return true; }
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
                                Source = chatShortcut.ImageSource
                            }
                        },
                        new StackLayout
                        {
                            Spacing = 5,
                            Children =
                            {
                                new Label
                                {
                                    Text = chatShortcut.Name,
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

            return newChatFrame;
        }

        private void StartChatBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(_pageFactory.CreatePage<StartChatPage>());
        }
    }
}
