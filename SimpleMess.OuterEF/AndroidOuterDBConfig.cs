using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleMess.OuterEF
{
    public class AndroidOuterDBConfig : IOuterDBConfig
    {
        public string GetOuterDBConnectionString()
        {
            return "Server=tcp:zdd-server.database.windows.net,1433;Initial Catalog=SimpleMessDB;Persist Security Info=False;User ID=zindan46;Password=SimpleMessDBPassword1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        }
    }
}
