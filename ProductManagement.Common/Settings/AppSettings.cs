using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Common.Settings
{
    public class AppSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; }
        public Authentication Authentication { get; set; }
    }

    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
        public string DesignTimeConnection { get; set; }
    }

    public class Authentication
    {
        public string[] AppGuid { get; set; }
    }
}
