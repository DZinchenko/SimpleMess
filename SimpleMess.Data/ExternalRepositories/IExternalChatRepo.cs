using SimpleMess.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleMess.Data.ExternalRepositories
{
    public interface IExternalChatRepo
    {
        void CreateChat(Chat chat);
        Chat GetСhatById(int id);
    }
}
