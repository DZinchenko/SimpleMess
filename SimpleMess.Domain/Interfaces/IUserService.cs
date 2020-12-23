﻿using System;
using System.Collections.Generic;
using System.Text;
using SimpleMess.Data.Entities;

namespace SimpleMess.Domain.Interfaces
{
    public interface IUserService
    {
        void Register(User user);
        User LogIn(string username, string password);
    }
}
