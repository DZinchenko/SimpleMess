using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SimpleMess.Data.Entities;
using SimpleMess.Data.ExternalRepositories;

namespace SimpleMess.OuterEF.Repositories
{
    public class ExternalUserRepo : IExternalUserRepo
    {
        public void CreateUser(User user)
        {
            using(var ctx = new OuterContext())
            {
                ctx.Users.Add(user);
                ctx.SaveChanges();
            }
        }

        public void UpdateUser(User user)
        {
            using (var ctx = new OuterContext())
            {
                ctx.Users.Attach(user);
                ctx.Entry(user).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public void DeleteUser(User user)
        {
            using (var ctx = new OuterContext())
            {
                ctx.Users.Remove(user);
                ctx.SaveChanges();
            }
        }

        public User GetUserById(int id)
        {
            using (var ctx = new OuterContext())
            {
                return ctx.Users.First(user=>user.Id == id);
            }
        }

        public User GetUserByUsername(string username)
        {
            using (var ctx = new OuterContext())
            {
                return ctx.Users.FirstOrDefault(user => user.Username == username);
            }
        }
    }
}
