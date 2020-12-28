using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SimpleMess.InnerEF
{
    public class AndroidInnerDBConfig : IInnerDBConfig
    {
        public string GetInnerDBPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "SimpleMess.db");
        }
    }
}
