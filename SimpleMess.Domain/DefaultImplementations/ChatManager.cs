using System;
using System.Collections.Generic;
using System.Text;
using SimpleMess.Data.Entities;
using SimpleMess.Domain.Interfaces;

namespace SimpleMess.Domain.DefaultImplementations
{
    public class ChatManager : IChatManager
    {
        private Chat _currentChat;

        public void ActivateChat(Chat chat)
        {
            _currentChat = chat;
        }

        public void DeactivateChat()
        {
            _currentChat = null;
        }

        public Chat GetActiveChat()
        {
            return _currentChat;
        }
    }
}
