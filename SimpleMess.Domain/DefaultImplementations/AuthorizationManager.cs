using System;
using System.Collections.Generic;
using System.Text;
using SimpleMess.Data.Entities;
using SimpleMess.Domain.Interfaces;
using SimpleMess.Data.ExternalRepositories;
using SimpleMess.Data.InternalRepositories;

namespace SimpleMess.Domain.DefaultImplementations
{
   public  class AuthorizationManager : IAuthorizationManager
    {
        private IExternalUserRepo _extUserRepo;
        private IInternalUserRepo _intUserRepo;
        private IInternalCurrUserInfoRepo _currUserInfoRepo;

        private User _currUser;

        public AuthorizationManager(IExternalUserRepo extUserRepo,
                                    IInternalUserRepo intUserRepo,
                                    IInternalCurrUserInfoRepo currUserInfoRepo)
        {
            _extUserRepo = extUserRepo;
            _intUserRepo = intUserRepo;
            _currUserInfoRepo = currUserInfoRepo;
        }

        public void AuthorizeUser(string login, string password)
        {
            var user = _extUserRepo.GetUserByUsername(login);

            if (user == null || user.Password != password)
            {
                throw new ArgumentException("Invalid username or password");
            }

            _currUserInfoRepo.CreateCurrUserInfo(new CurrUserInfo { UserId = user.Id, LastUpdateTime = DateTime.MinValue });
            _currUser = user;
            _intUserRepo.CreateUser(user);
        }

        public void DeauthorizeUser()
        {
            _currUser = null;
            _currUserInfoRepo.DeleteCurrUserInfo();
        }

        public User GetAuthorizedUser()
        {
            if (_currUser != null)
            {
                return _currUser;
            }
            else
            {
                var currUserInfo = _currUserInfoRepo.GetCurrUserInfo();
                if (currUserInfo != null)
                {
                    _currUser = _intUserRepo.GetUserById(currUserInfo.UserId);
                }
                return _currUser;
            }
        }
    }
}
