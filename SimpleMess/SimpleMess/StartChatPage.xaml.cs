using SimpleMess.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SimpleMess.Domain.Interfaces;
using SimpleMess.Data.ExternalRepositories;
using Plugin.Media.Abstractions;
using Plugin.Media;

namespace SimpleMess
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartChatPage : TabbedPage
    {
        private IChatService _chatService;
        private IExternalUserRepo _extUserRepo;

        private User _privateChatSecondUser;
        private List<User> _groupChatUsers;

        private MediaFile _avatar;
        private Image _avatarImage;

        public StartChatPage(IChatService chatService,
                             IExternalUserRepo extUserRepo)
        {
            _chatService = chatService;
            _extUserRepo = extUserRepo;

            InitializeComponent();

            _groupChatUsers = new List<User>();
        }

        private void PrivateChatUserSelectBtn_Clicked(object sender, EventArgs e)
        {
            _privateChatSecondUser =  _extUserRepo.GetUserByUsername(PrivateChatUserEntry.Text);
            if (_privateChatSecondUser == null)
            {
                DisplayAlert("Alert", "No such user exists", "OK");
            }
        }

        private void StartPrivateChatBtn_Clicked(object sender, EventArgs e)
        {
            if (_privateChatSecondUser != null)
            {
                _chatService.StartPrivateChat(_privateChatSecondUser);
                Navigation.PopModalAsync();
            }
            else
            {
                DisplayAlert("Alert", "Select second user", "OK");
            }
        }

        private void GroupChatUserSelectBtn_Clicked(object sender, EventArgs e)
        {
            var addedUser = _extUserRepo.GetUserByUsername(GroupChatUserEntry.Text);
            if (addedUser != null)
            {
                _groupChatUsers.Add(addedUser);
                AddedUsersLayout.Children.Add(new Label { Text = addedUser.Username, TextColor = Color.Black });
            }
            else
            {
                DisplayAlert("Alert", "No such user exists", "OK");
            }
        }

        private void StartGroupChatBtn_Clicked(object sender, EventArgs e)
        {
            if (_groupChatUsers.Count() > 2)
            {
                if (_avatar != null)
                {
                    var photo = new byte[_avatar.GetStream().Length];
                    _avatar.GetStream().Read(photo, 0, Convert.ToInt32(_avatar.GetStream().Length));
                    _chatService.StartGroupChat(_groupChatUsers, ChatNameEntry.Text, photo);
                }
                else
                {
                    _chatService.StartGroupChat(_groupChatUsers, ChatNameEntry.Text, null);
                }
                Navigation.PopModalAsync();
            }
            else
            {
                DisplayAlert("Alert", "Add more users", "OK");
            }
        }

        private async void PicturePickBtn_Clicked(object sender, EventArgs e)
        {
            _avatar = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions());
            if (_avatarImage != null) { AvatarPickLayout.Children.Remove(_avatarImage); }
            _avatarImage = new Image
            {
                HeightRequest = 150,
                WidthRequest = 150,
                Source = _avatar.Path,
            };

            AvatarPickLayout.Children.Add(_avatarImage);
        }
    }
}