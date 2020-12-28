using SimpleMess.Data.Entities;
using SimpleMess.Data.InternalRepositories;
using SimpleMess.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace SimpleMess.ChatShortcutBuildStrategy
{
    public class BuildPrivateChatShortcutStrategy : IBuildChatShortcutStrategy
    {
        private IInternalUserRepo _userRepo;
        private IAuthorizationManager _authManager;

        public BuildPrivateChatShortcutStrategy(IInternalUserRepo intUserRepo,
                                                IAuthorizationManager authManager)
        {
            _userRepo = intUserRepo;
            _authManager = authManager;
        }

        public ChatShortcutDTO ExtractChatShortcut(Chat chat)
        {
            var chatShortcutDTO = new ChatShortcutDTO();
            var secondUser = _userRepo.GetUserById(chat.UserChats.Select(uc => uc.UserId).Where(id => id != _authManager.GetAuthorizedUser().Id).First());

            if (secondUser.Photo != null)
            {
                chatShortcutDTO.ImageSource = ImageSource.FromStream(() => new MemoryStream(secondUser.Photo));
            }
            else
            {
                //need to add default pic
                chatShortcutDTO.ImageSource = null;
            }
            chatShortcutDTO.Name = secondUser.Username;

            return chatShortcutDTO;
        }
    }
}
