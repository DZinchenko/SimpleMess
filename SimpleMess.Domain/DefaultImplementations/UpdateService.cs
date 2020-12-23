using System;
using System.Collections.Generic;
using System.Text;
using SimpleMess.Domain.Interfaces;
using SimpleMess.Data.ExternalRepositories;
using SimpleMess.Data.InternalRepositories;
using Xamarin.Essentials;
using SimpleMess.Data.Entities;
using System.Linq;

namespace SimpleMess.Domain.DefaultImplementations
{
    public class UpdateService : IUpdateService
    {
        private IInternalUserRepo _intUserRepo;
        private IExternalUserRepo _extUserRepo;
        private IInternalChatRepo _intChatRepo;
        private IExternalChatRepo _extChatRepo;
        private IInternalMessageRepo _intMsgRepo;
        private IExternalMessageRepo _extMsgRepo;
        private IInternalCurrUserInfoRepo _intCurrUserInfoRepo;

        public UpdateService(IInternalUserRepo intUserRepo,
                             IExternalUserRepo extUserRepo,
                             IInternalChatRepo intChatRepo,
                             IExternalChatRepo extChatRepo,
                             IInternalMessageRepo intMsgRepo,
                             IExternalMessageRepo extMsgRepo,
                             IInternalCurrUserInfoRepo intCurrUserInfoRepo)
        {
            _intUserRepo = intUserRepo;
            _extUserRepo = extUserRepo;
            _intChatRepo = intChatRepo;
            _extChatRepo = extChatRepo;
            _intMsgRepo = intMsgRepo;
            _extMsgRepo = extMsgRepo;
            _intCurrUserInfoRepo = intCurrUserInfoRepo;
        }

        public bool Update()
        {
            if(Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                throw new Exception("Bad internet connection");
            }

            var updateTime = DateTime.Now;
            var currUserInfo = _intCurrUserInfoRepo.GetCurrUserInfo();
            currUserInfo.LastUpdateTime = updateTime;
            var newMsgs = _extMsgRepo.GetNewMessages(currUserInfo.UserId, updateTime);
            _intMsgRepo.CreateMessages(newMsgs);

            var newChats = new List<Chat>();
            foreach(int chatId in newMsgs.Select(msg => msg.ChatId).Distinct())
            {
                if (_intChatRepo.GetChatById(chatId) == null)
                {
                    _intChatRepo.CreateChat(_extChatRepo.GetСhatById(chatId));
                }
            }

            var newUsers = new List<User>();
            foreach (int userId in newChats.SelectMany(chat => chat.UserChats).Select(uc=>uc.UserId).Distinct())
            {
                if (_intUserRepo.GetUserById(userId) == null)
                {
                    _intUserRepo.CreateUser(_extUserRepo.GetUserById(userId));
                }

            }

            _intCurrUserInfoRepo.UpdateCurrUserInfo(currUserInfo);

            return true;
        }
    }
}
