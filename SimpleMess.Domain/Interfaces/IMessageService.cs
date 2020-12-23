using SimpleMess.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleMess.Domain.Interfaces
{
    public interface IMessageService
    {
        void SendMessage(Message message);
        void UserSeenMessages(List<Message> messages, User user);
    }
}
