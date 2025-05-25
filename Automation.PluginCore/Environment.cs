using Automation.PluginCore.Interface;
using Automation.PluginCore.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.PluginCore
{
    public class Environment
    {
        private static readonly Environment _instance = new Environment();
        public static Environment Instance => _instance;

        public string Line { get; set; }
        public string Process { get; set; }
        public string Model { get; set; }

        public Dictionary<Guid, INode> CurrentNodes { get; set; }
        public ObservableCollection<string> ActivePlugins => new ObservableCollection<string>(PluginManager.Assemblies.Select(a => a.FullName));
    }
}
