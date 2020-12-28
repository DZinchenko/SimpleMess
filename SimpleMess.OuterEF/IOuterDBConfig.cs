using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SimpleMess.OuterEF
{
    public interface IOuterDBConfig
    {
        string GetOuterDBConnectionString();
    }
}
