using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHalas.WSI.Lab1.Configuration
{
    public static class DBConfig
    {
        public static string DatabaseName { get; } = "WSI";
        public static string ConnectionString { get; } = "mongodb://localhost:32775";
    }
}
