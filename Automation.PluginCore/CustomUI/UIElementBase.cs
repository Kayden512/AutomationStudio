using Automation.PluginCore.Base;
using Automation.PluginCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.PluginCore.CustomUI
{
    public abstract class UIElementBase : ViewModelBase
    {
        public double X { get; set; } = 100;
        public double Y { get; set; } = 100;
        public double Width { get; set; } = 200;
        public double Height { get; set; } = 50;

    }
}
