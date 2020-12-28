using SimpleMess.Data.InternalRepositories;
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
        private IUserService _userService;
        private IInternalCurrUserInfoRepo _currUserInfoRepo;
        private IInternalUserRepo _intUserRepo;
        private IPageFactory _pageFactory;
        private IAuthorizationManager _authManager;

        public LoginPage(IUserService userService,
                         IInternalCurrUserInfoRepo currUserInfoRepo,
                         IInternalUserRepo intUserRepo,
                         IPageFactory pageFactory,
                         IAuthorizationManager authManager)
        {
            _userService = userService;
            _currUserInfoRepo = currUserInfoRepo;
            _intUserRepo = intUserRepo;
            _pageFactory = pageFactory;
            _authManager = authManager;

            InitializeComponent();
        }

        private void RegisterBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(_pageFactory.CreatePage<RegistrationPage>());
        }

        private void LogInBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                _authManager.AuthorizeUser(UsernameEntry.Text, PasswordEntry.Text);
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