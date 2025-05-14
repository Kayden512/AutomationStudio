using Automation.PluginCore.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.PluginCore.Base.Device.PLC
{
    public class TestTool : DeviceBase
    {
        public double Double { get; set; }
        public int Int { get; set; }

        public ObservableCollection<INode> Collection { get; set; } = new ObservableCollection<INode>(); 

    }
}
