﻿using SimpleMess.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleMess.Data.ExternalRepositories
{
    public interface IExternalUserRepo
    {
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
        User GetUserById(int id);
        User GetUserByUsername(string username);
    }
}
