using SimpleMess.Data.Entities;
using SimpleMess.Data.ExternalRepositories;
using SimpleMess.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SimpleMess.Data.InternalRepositories;

namespace SimpleMess.Domain.DefaultImplementations
{
    public class ChatService : IChatService
    {
        private IExternalChatRepo _extChatRepo;
        private IInternalChatRepo _intChatRepo;
        private IInternalMessageRepo _intMsgRepo;
        private IAuthorizationManager _authManager;

        public ChatService(IExternalChatRepo extChatRepo,
                           IInternalChatRepo intChatRepo,
                           IInternalMessageRepo intMsgRepo,
                           IAuthorizationManager authManager)
        {
            _extChatRepo = extChatRepo;
            _intChatRepo = intChatRepo;
            _intMsgRepo = intMsgRepo;
            _authManager = authManager;
        }

        public List<Chat> GetChatsWithMsgAfterTime(DateTime time)
        {
            return _intChatRepo.GetAllChatsForUser(_authManager.GetAuthorizedUser().Id)
                .Where(chat => _intMsgRepo.GetLastMessageTimeInChat(chat.Id) > time)
                .OrderByDescending(chat => _intMsgRepo.GetLastMessageTimeInChat(chat.Id)).ToList();
        }

        public void StartGroupChat(List<User> users, string chatName, byte[] chatImage)
        {
            var chat = new GroupChat { Name = chatName, Picture = chatImage };
            users.Add(_authManager.GetAuthorizedUser());
            chat.UserChats = users.Select(user => new UserChat { Chat = chat, UserId = user.Id }).ToList();
            _extChatRepo.CreateChat(chat);
        }

        public void StartPrivateChat(User secondUser)
        {
            var chat = new Chat();
            chat.UserChats = new List<UserChat>
            {
                new UserChat
                {
                    Chat = chat,
                    UserId = _authManager.GetAuthorizedUser().Id
                },
                new UserChat
                {
                    Chat = chat,
                    UserId = secondUser.Id
                }
            };
        }
    }
}
