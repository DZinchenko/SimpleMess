using SimpleMess.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleMess.ChatShortcutBuildStrategy
{
    public interface IBuildChatShortcutStrategy
    {
        ChatShortcutDTO ExtractChatShortcut(Chat chat);
    }
}
