using System;
using System.Collections.Generic;
using System.Text;
using SimpleMess.Data.InternalRepositories;
using SimpleMess.Data.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SimpleMess.InnerEF.Repositories
{
    public class CurrUserInfoRepo : IInternalCurrUserInfoRepo
    {
        public void CreateCurrUserInfo(CurrUserInfo currUserInfo)
        {
            using (var ctx = new InnerContext())
            {
                if (ctx.CurrUserInfo.ToList().Count == 0)
                {
                    ctx.CurrUserInfo.Add(currUserInfo);
                    ctx.SaveChanges();
                }
                else
                {
                    throw new ApplicationException("There is CurrUserInfo in repository. Only one instance of CurrUserInfo can exist");
                }
            }
        }

        public void UpdateCurrUserInfo(CurrUserInfo currUserInfo)
        {
            using (var ctx = new InnerContext())
            {
                ctx.CurrUserInfo.Attach(currUserInfo);
                ctx.Entry(currUserInfo).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public void DeleteCurrUserInfo()
        {
            using (var ctx = new InnerContext())
            {
                ctx.CurrUserInfo.Remove(GetCurrUserInfo());
                ctx.SaveChanges();
            }
        }

        public CurrUserInfo GetCurrUserInfo()
        {
            using (var ctx = new InnerContext())
            {
                return ctx.CurrUserInfo.ToList().FirstOrDefault();
            }
        }
    }
}
