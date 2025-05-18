using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.PluginCore
{
    public class Access
    {
        private static readonly Access _instance = new Access();
        public static Access Instance => _instance;
    }
}
