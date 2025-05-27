using Automation.PluginCore.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.PluginCore.Base.Device.PLC.Resource
{
    public class Memory : NodeBase, IGroup
    {
        string _address;
        public uint MemotyLength = 16;

        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }
    }
}
