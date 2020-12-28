using SimpleMess.Data.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace SimpleMess.ChatShortcutBuildStrategy
{
    public class BuildGroupChatShortcutStrategy : IBuildChatShortcutStrategy
    {
        public ChatShortcutDTO ExtractChatShortcut(Chat chat)
        {
            var groupChat = (GroupChat)chat;
            var chatShortcutDTO = new ChatShortcutDTO();

            if (groupChat.Picture != null)
            {
                chatShortcutDTO.ImageSource = ImageSource.FromStream(() => new MemoryStream(groupChat.Picture));
            }
            else
            {
                //need to add a default pic
                chatShortcutDTO.ImageSource = null;
            }
            chatShortcutDTO.ImageSource = groupChat.Name;

            return chatShortcutDTO;
        }
    }
}
