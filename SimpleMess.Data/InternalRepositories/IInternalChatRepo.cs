using SimpleMess.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleMess.Data.InternalRepositories
{
    public interface IInternalChatRepo
    {
        void CreateChat(Chat chat);
        void UpdateChat(Chat chat);
        void DeleteChat(Chat chat);
        Chat GetChatById(int id);
        List<Chat> GetAllChatsForUser(int userId);
    }
}
