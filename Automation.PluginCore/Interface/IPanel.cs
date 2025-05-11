using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.PluginCore.Interface
{
    public interface IPanel : IViewModel
    {
        INode SelectedNode { get; set; }
    }
}
