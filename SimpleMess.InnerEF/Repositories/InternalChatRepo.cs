using System;
using System.Collections.Generic;
using System.Text;
using SimpleMess.Data.InternalRepositories;
using SimpleMess.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SimpleMess.InnerEF.Repositories
{
    public class InternalChatRepo : IInternalChatRepo
    {
        public void CreateChat(Chat chat)
        {
            using (var ctx = new InnerContext())
            {
                ctx.Chats.Add(chat);
                ctx.SaveChanges();
            }
        }

        public void DeleteChat(Chat chat)
        {
            using (var ctx = new InnerContext())
            {
                ctx.Chats.Remove(chat);
                ctx.SaveChanges();
            }
        }

        public void UpdateChat(Chat chat)
        {
            using (var ctx = new InnerContext())
            {
                ctx.Chats.Attach(chat);
                ctx.Entry(chat).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public List<Chat> GetAllChatsForUser(int userId)
        {
            using (var ctx = new InnerContext())
            {
                return ctx.Chats.Where(chat => chat.UserChats.Exists(uc => uc.UserId == userId)).Include(chat=>chat.UserChats).ToList();
            }
        }

        public Chat GetChatById(int id)
        {
            using (var ctx = new InnerContext())
            {
                return ctx.Chats.Include(chat=>chat.UserChats).FirstOrDefault(chat => chat.Id == id);
            }
        }
    }
}
