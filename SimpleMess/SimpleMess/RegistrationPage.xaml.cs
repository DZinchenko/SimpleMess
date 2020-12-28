using Plugin.Media;
using Plugin.Media.Abstractions;
using SimpleMess.Data.Entities;
using SimpleMess.Data.ExternalRepositories;
using SimpleMess.Domain.Interfaces;
using System;
using System.IO;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimpleMess
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        private IUserService _userService;

        private MediaFile _avatar;
        private Image _avatarImage;

        public RegistrationPage(IUserService userService)
        {
            _userService = userService;

            InitializeComponent();
        }

        private void BackBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void RegisterBtn_Clicked(object sender, EventArgs e)
        {
            if (UsernameEntry.Text.Length != 0 && PhoneEntry.Text.Length != 0 && PasswordEntry.Text.Length != 0)
            {
                User user = new User
                {
                    Username = UsernameEntry.Text,
                    Phone = PhoneEntry.Text,
                    Password = PasswordEntry.Text,
                };

                if (_avatar != null)
                {
                    user.Photo = new byte[_avatar.GetStream().Length];
                    _avatar.GetStream().Read(user.Photo, 0, Convert.ToInt32(_avatar.GetStream().Length));
                }

                try
                {
                    _userService.Register(user);
                    Navigation.PopModalAsync();
                }
                catch (ArgumentException ex)
                {
                    ErrorLabelFrame.IsVisible = true;
                    ErrorLabel.Text = ex.Message;
                }
            }
            else
            {
                ErrorLabelFrame.IsVisible = true;
                ErrorLabel.Text = "Enter all the information";
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