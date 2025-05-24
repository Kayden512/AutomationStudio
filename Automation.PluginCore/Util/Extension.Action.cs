using Automation.PluginCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.PluginCore.Util
{
    public static partial class Extension
    {
        public static List<IAction> GetAction(Type[] types = null)
        {
            return Main.GetAction(types);
        }
    }
}
