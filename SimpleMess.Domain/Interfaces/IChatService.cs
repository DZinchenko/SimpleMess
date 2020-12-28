using SimpleMess.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleMess.Domain.Interfaces
{
    public interface IChatService
    {
        void StartPrivateChat(User secondUser);
        void StartGroupChat(List<User> users, string chatName, byte[] chatImage);
        List<Chat> GetChatsWithMsgAfterTime(DateTime time);
    }
}
