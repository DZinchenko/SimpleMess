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
using SimpleMess.Data.ExternalRepositories;
using SimpleMess.Data.InternalRepositories;
using SimpleMess.OuterEF;
using SimpleMess.InnerEF;
using SimpleMess.MainPageFunctionality;

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
            Kernel.Bind<IMessageService>().To<MessageService>();
            Kernel.Bind<IUpdateService>().To<UpdateService>();

            Kernel.Bind<IExternalUserRepo>().To<OuterEF.Repositories.ExternalUserRepo>();
            Kernel.Bind<IExternalChatRepo>().To<OuterEF.Repositories.ExternalChatRepo>();
            Kernel.Bind<IExternalMessageRepo>().To<OuterEF.Repositories.ExternalMessageRepo>();

            Kernel.Bind<IInternalUserRepo>().To<InnerEF.Repositories.InternalUserRepo>();
            Kernel.Bind<IInternalChatRepo>().To<InnerEF.Repositories.InternalChatRepo>();
            Kernel.Bind<IInternalMessageRepo>().To<InnerEF.Repositories.InternalMessageRepo>();

            Kernel.Bind<IPageFactory>().To<DefaultPageFactory>();
            Kernel.Bind<IBuildChatShortcutStrategyFactory>().To<BuildChatShortcutStrategyFactory>();

            Kernel.Bind<IAuthorizationManager>().To<AuthorizationManager>();
            Kernel.Bind<IChatManager>().To<ChatManager>().InSingletonScope();

            Kernel.Bind<IOuterDBConfig>().To<AndroidOuterDBConfig>();
            Kernel.Bind<IInnerDBConfig>().To<AndroidInnerDBConfig>();
        }
    }
}