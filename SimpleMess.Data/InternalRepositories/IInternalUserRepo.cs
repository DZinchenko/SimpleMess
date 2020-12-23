using SimpleMess.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleMess.Data.InternalRepositories
{
    public interface IInternalUserRepo
    {
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
        User GetUserById(int id);
        List<User> GetAllUsers();
        List<User> GetUsersInChat(int chatId);
    }
}
