using SimpleMess.Domain.Interfaces;
using SimpleMess.InnerEF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimpleMess
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        IUserService _userService;

        public LoginPage(IUserService userService)
        {
            _userService = userService;

            InitializeComponent();
        }

        private void RegisterBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(DependencyService.Resolve<RegistrationPage>());
        }

        private void LogInBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.CurrentUser = _userService.LogIn(UsernameEntry.Text, PasswordEntry.Text);
                Navigation.PushModalAsync(DependencyService.Resolve<MainPage>());
            }
            catch (ArgumentException ex)
            {
                ErrorLabelFrame.IsVisible = true;
                ErrorLabel.Text = ex.Message;
            }
        }
    }
}