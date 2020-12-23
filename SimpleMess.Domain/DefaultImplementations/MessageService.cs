using SimpleMess.Data.Entities;
using SimpleMess.Data.ExternalRepositories;
using SimpleMess.Data.InternalRepositories;
using SimpleMess.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleMess.Domain.DefaultImplementations
{
    class MessageService : IMessageService
    {
        private IExternalMessageRepo _extMsgRepo;
        private IInternalMessageRepo _intMsgRepo;

        public MessageService(IExternalMessageRepo extMsgRepo,
                              IInternalMessageRepo intMsgRepo)
        {
            _extMsgRepo = extMsgRepo;
            _intMsgRepo = intMsgRepo;
        }
        public void SendMessage(Message message)
        {
            _extMsgRepo.CreateMessage(message);
        }

        public void UserSeenMessages(List<Message> messages, User user)
        {
            messages.ForEach(msg => msg.UserSeenMessages.Add(new UserSeenMessage { Message = msg, MessageId = msg.Id, UserId = user.Id }));
            _extMsgRepo.UpdateMessages(messages);
            _intMsgRepo.UpdateMessages(messages);
        }
    }
}
