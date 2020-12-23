using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Ninject;
using SimpleMess.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleMess.Domain.DefaultImplementations;
using SimpleMess.Data.Repositories;
using SimpleMess.OuterEF;

namespace SimpleMess.Droid
{
    class NinjectDroid
    {
        public static IKernel Kernel;

        public static void RegisterServices()
        {
            Kernel = new StandardKernel();

            Kernel.Bind<IUserService>().To<UserService>();
            Kernel.Bind<IChatService>().To<ChatService>();

            Kernel.Bind<IUserRepository>().To<OuterEF.Repositories.OuterUserRepository>().WhenInjectedExactlyInto<RegistrationPage>();
            //Kernel.Bind<IUserService>().To<UserService>();
        }
    }
}