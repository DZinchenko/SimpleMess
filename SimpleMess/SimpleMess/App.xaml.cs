using SimpleMess.Data.Entities;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace SimpleMess
{
    public partial class App : Application
    {
        public static User CurrentUser { get; set; }
        public static Chat CurrentChat { get; set; }

        public App(Func<Type,object> func)
        {
            InitializeComponent();
            DependencyResolver.ResolveUsing(func);
            MainPage = DependencyService.Resolve<LoginPage>();
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
