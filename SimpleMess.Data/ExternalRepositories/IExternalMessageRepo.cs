using SimpleMess.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleMess.Data.ExternalRepositories
{
    public interface IExternalMessageRepo
    {
        void CreateMessage(Message message);
        void UpdateMessages(List<Message> messages);
        List<Message> GetNewMessages(int userId, DateTime lastUpdateTime);
    }
}
