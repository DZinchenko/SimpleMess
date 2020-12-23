using SimpleMess.Data.Entities;
using SimpleMess.Data.ExternalRepositories;
using SimpleMess.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleMess.Domain.DefaultImplementations
{
    public class ChatService : IChatService
    {
        private IExternalChatRepo _chatRepo;

        public ChatService(IExternalChatRepo outerChatRepo)
        {
            _chatRepo = outerChatRepo;
        }

        public void StartChat(Chat chat)
        {
            _chatRepo.CreateChat(chat);
        }
    }
}
