using SimpleMess.Data.Entities;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using SimpleMess.Domain.Interfaces;

namespace SimpleMess
{
    public partial class App : Application
    {
        public App(IAuthorizationManager authManager, IPageFactory pageFactory)
        {
            InitializeComponent();
            MainPage = pageFactory.CreatePage<LoginPage>();
            if (authManager.GetAuthorizedUser() != null)
            {
                MainPage.Navigation.PushModalAsync(pageFactory.CreatePage<LoginPage>());
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
