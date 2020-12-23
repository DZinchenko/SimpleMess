using SimpleMess.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleMess.Data.InternalRepositories
{
    public interface IInternalCurrUserInfoRepo
    {
        void CreateCurrUserInfo(CurrUserInfo currUserInfo);
        void UpdateCurrUserInfo(CurrUserInfo currUserInfo);
        void DeleteCurrUserInfo(CurrUserInfo currUserInfo);
        CurrUserInfo GetCurrUserInfo();
    }
}
