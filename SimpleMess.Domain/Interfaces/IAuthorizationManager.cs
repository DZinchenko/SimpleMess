using SimpleMess.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleMess.Domain.Interfaces
{
    public interface IAuthorizationManager
    {
        void AuthorizeUser(string login, string password);
        void DeauthorizeUser();
        User GetAuthorizedUser();
    }
}
