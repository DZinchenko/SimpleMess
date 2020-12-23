using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SimpleMess.Data.Entities;
using SimpleMess.Data.InternalRepositories;

namespace SimpleMess.InnerEF.Repositories
{
    public class InternalUserRepo : IInternalUserRepo
    {
        public void CreateUser(User user)
        {
            using(var ctx = new InnerContext())
            {
                ctx.Users.Add(user);
                ctx.SaveChanges();
            }
        }

        public void DeleteUser(User user)
        {
            using(var  ctx = new InnerContext())
            {
                ctx.Users.Remove(user);
                ctx.SaveChanges();
            }
        }

        public void UpdateUser(User user)
        {
            using (var ctx = new InnerContext())
            {
                ctx.Users.Attach(user);
                ctx.Entry(user).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public List<User> GetAllUsers()
        {
            using (var ctx = new InnerContext())
            {
                return ctx.Users.ToList();
            }
        }

        public User GetUserById(int id)
        {
            using (var ctx = new InnerContext())
            {
                return ctx.Users.First(u => u.Id == id);
            }
        }

        public List<User> GetUsers()
        {
            using (var ctx = new InnerContext())
            {
                return ctx.Users.ToList();
            }
        }

        public List<User> GetUsersInChat(int chatId)
        {
            using (var ctx = new InnerContext())
            {
                return ctx.Chats.First(c => c.Id == chatId).UserChats.Select(uc => uc.UserId)
                    .Join(ctx.Users, userId => userId, user => user.Id, (userId, user) => user).ToList();
            }
        }
    }
}
