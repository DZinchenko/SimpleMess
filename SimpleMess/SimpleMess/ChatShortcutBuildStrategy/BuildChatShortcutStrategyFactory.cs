using SimpleMess.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SimpleMess.ChatShortcutBuildStrategy
{
    public class BuildChatShortcutStrategyFactory : IBuildChatShortcutStrategyFactory
    {
        public IBuildChatShortcutStrategy Create(Chat chat)
        {
            if (chat.GetType() == typeof(GroupChat))
            {
                return DependencyService.Resolve<BuildGroupChatShortcutStrategy>();
            }
            else
            {
                return DependencyService.Resolve<BuildPrivateChatShortcutStrategy>();
            }
        }
    }
}
