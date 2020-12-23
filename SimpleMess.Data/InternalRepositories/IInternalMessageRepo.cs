using SimpleMess.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleMess.Data.InternalRepositories
{
    public interface IInternalMessageRepo
    {
        void CreateMessage(Message message);
        void DeleteMessage(Message message);
        void UpdateMessage(Message message);
        void CreateMessages(List<Message> messages);
        void DeleteMessages(List<Message> messages);
        void UpdateMessages(List<Message> messages);
        List<Message> GetMessagesInChat(int chatId);
        Message GetLastMessageInChat(int chatId);
        List<Message> GetNewMessagesInChat(int chatId, Message lastMsg);
    }
}
