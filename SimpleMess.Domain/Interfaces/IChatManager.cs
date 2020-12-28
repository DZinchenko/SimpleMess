using System;
using System.Collections.Generic;
using System.Text;
using SimpleMess.Data.Entities;

namespace SimpleMess.Domain.Interfaces
{
    public interface IChatManager
    {
        Chat GetActiveChat();
        void ActivateChat(Chat chat);
        void DeactivateChat();
    }
}
