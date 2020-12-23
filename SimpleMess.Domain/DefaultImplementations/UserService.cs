using SimpleMess.Data.Entities;
using SimpleMess.Data.ExternalRepositories;
using SimpleMess.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleMess.Domain.DefaultImplementations
{
    public class UserService : IUserService
    {
        private IExternalUserRepo _userRepo;

        public UserService(IExternalUserRepo outerUserRepo)
        {
            _userRepo = outerUserRepo;
        }

        public User LogIn(string username, string password)
        {
            var user = _userRepo.GetUserByUsername(username);
            if(user != null && user.Password == password)
            {
                return user;
            }
            else
            {
                throw new ArgumentException("Invalid username or password");
            }
        }

        public void Register(User user)
        {
            if (_userRepo.GetUserByUsername(user.Username) == null)
            {
                _userRepo.CreateUser(user);
            }
            else
            {
                throw new ArgumentException("User with such username already exists");
            }
        }
    }
}
