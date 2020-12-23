using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SimpleMess.Data.Entities;
using SimpleMess.Data.ExternalRepositories;

namespace SimpleMess.OuterEF.Repositories
{
    class ExternalChatRepo : IExternalChatRepo
    {
        public void CreateChat(Chat chat)
        {
            using (var ctx = new OuterContext())
            {
                ctx.Chats.Add(chat);
                ctx.SaveChanges();
            }
        }

        public void DeleteChat(Chat chat)
        {
            using (var ctx = new OuterContext())
            {
                ctx.Chats.Remove(chat);
                ctx.SaveChanges();
            }
        }

        public void UpdateChat(Chat chat)
        {
            using (var ctx = new OuterContext())
            {
                ctx.Chats.Attach(chat);
                ctx.Entry(chat).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public Chat GetСhatById(int id)
        {
            using (var ctx = new OuterContext())
            {
                return ctx.Chats.Include(chat=>chat.UserChats).FirstOrDefault(chat => chat.Id == id);
            }
        }
    }
}
