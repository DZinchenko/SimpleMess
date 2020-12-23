using SimpleMess.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleMess.Domain.Interfaces
{
    public interface IChatService
    {
        void StartChat(Chat chat);
    }
}
