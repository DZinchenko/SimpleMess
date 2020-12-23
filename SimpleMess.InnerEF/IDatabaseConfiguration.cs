using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SimpleMess.InnerEF
{
    interface IDatabaseConfiguration
    {
        string GetDatabasePath();
    }

    class AndroidDatabaseConfiguration : IDatabaseConfiguration
    {
        public string GetDatabasePath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "SimpleMess.db");
        }
    }
}
