using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SimpleMess.Data.Entities;
using SimpleMess.Data.ExternalRepositories;

namespace SimpleMess.OuterEF.Repositories
{
    class ExternalMessageRepo : IExternalMessageRepo
    {
        public void CreateMessage(Message message)
        {
            using (var ctx = new OuterContext())
            {
                ctx.Messages.Add(message);
                ctx.SaveChanges();
            }
        }

        public void DeleteMessage(Message message)
        {
            using (var ctx = new OuterContext())
            {
                ctx.Messages.Remove(message);
                ctx.SaveChanges();
            }
        }

        public void UpdateMessage(Message message)
        {
            using (var ctx = new OuterContext())
            {
                ctx.Messages.Attach(message);
                ctx.Entry(message).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public void CreateMessages(List<Message> messages)
        {
            using (var ctx = new OuterContext())
            {
                ctx.Messages.AddRange(messages);
                ctx.SaveChanges();
            }
        }

        public void DeleteMessages(List<Message> messages)
        {
            using (var ctx = new OuterContext())
            {
                ctx.Messages.RemoveRange(messages);
                ctx.SaveChanges();
            }
        }

        public void UpdateMessages(List<Message> messages)
        {
            using (var ctx = new OuterContext())
            {
                ctx.Messages.AttachRange(messages);
                messages.ForEach(msg => ctx.Entry(msg).State = EntityState.Modified);
                ctx.SaveChanges();
            }
        }

        public List<Message> GetNewMessages(int userId, DateTime lastUpdateTime)
        {
            using (var ctx = new OuterContext())
            {
                return ctx.Messages.Where(msg => msg.UserSeenMessages.Exists(usm => usm.UserId == userId) && msg.Time > lastUpdateTime).Include(msg=>msg.UserSeenMessages).ToList();
            }
        }
    }
}
