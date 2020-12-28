using System;
using System.Collections.Generic;
using System.Text;
using SimpleMess.Data.Entities;
using SimpleMess.Data.InternalRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SimpleMess.InnerEF.Repositories
{
    public class InternalMessageRepo : IInternalMessageRepo
    {
        public void CreateMessage(Message message)
        {
            using(var ctx = new InnerContext())
            {
                ctx.Messages.Add(message);
                ctx.SaveChanges();
            }
        }

        public void DeleteMessage(Message message)
        {
            using (var ctx = new InnerContext())
            {
                ctx.Messages.Remove(message);
                ctx.SaveChanges();
            }
        }

        public void UpdateMessage(Message message)
        {
            using (var ctx = new InnerContext())
            {
                ctx.Messages.Attach(message);
                ctx.Entry(message).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public List<Message> GetMessagesInChat(int chatId)
        {
            using (var ctx = new InnerContext())
            {
                return ctx.Messages.Where(msg => msg.ChatId == chatId).OrderBy(msg => msg.Time).Include(msg => msg.UserSeenMessages).ToList();
            }
        }

        public Message GetLastMessageInChat(int chatId)
        {
            using (var ctx = new InnerContext())
            {
                return ctx.Messages.Where(msg => msg.ChatId == chatId).OrderBy(msg => msg.Time).Include(msg => msg.UserSeenMessages).FirstOrDefault();
            }
        }

        public List<Message> GetNewMessagesInChat(int chatId, Message lastMsg)
        {
            if (lastMsg != null)
            {
                using (var ctx = new InnerContext())
                {
                    return ctx.Messages.Where(msg => msg.ChatId == chatId && msg.Id > lastMsg.Id).OrderBy(msg => msg.Time).Include(msg => msg.UserSeenMessages).ToList();
                }
            }
            else
            {
                throw new ArgumentException("lastMsg is null");
            }
        }

        public void CreateMessages(List<Message> messages)
        {
            using (var ctx = new InnerContext())
            {
                ctx.Messages.AddRange(messages);
                ctx.SaveChanges();
            }
        }

        public void DeleteMessages(List<Message> messages)
        {
            using (var ctx = new InnerContext())
            {
                ctx.Messages.RemoveRange(messages);
                ctx.SaveChanges();
            }
        }

        public void UpdateMessages(List<Message> messages)
        {
            using (var ctx = new InnerContext())
            {
                ctx.Messages.AttachRange(messages);
                messages.ForEach(msg => ctx.Entry(msg).State = EntityState.Modified);
                ctx.SaveChanges();
            }
        }

        public DateTime GetLastMessageTimeInChat(int chatId)
        {
            using (var ctx = new InnerContext())
            {
                var lastMessage = ctx.Messages.Where(msg => msg.ChatId == chatId).OrderBy(msg => msg.Time).Include(msg => msg.UserSeenMessages).FirstOrDefault();
                if (lastMessage != null)
                {
                    return lastMessage.Time;
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
        }
    }
}
