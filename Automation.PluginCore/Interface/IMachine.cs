using Automation.PluginCore.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.PluginCore.Interface
{
    public interface IMachine : IDevice, IGroup
    {
        NodeCollection Schedules { get; }
    }
}
