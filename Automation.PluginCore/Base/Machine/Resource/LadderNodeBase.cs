using Automation.PluginCore.Control.PropertyGrid;
using Automation.PluginCore.Interface;
using Automation.PluginCore.Util.Extension;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Automation.PluginCore.Base.Machine.Resource
{
    [CategoryOrder("Ladder",2)]
    public abstract class LadderNodeBase : NodeBase, ILadder
    {
        int _x;
        int _y;
        bool _flow;
        bool _verticalLine;

        public override string Icon => "M14 1V3H10V1H8V22H10V20H14V22H16V1H14M14 5V8H10V5H14M14 10V13H10V10H14M10 18V15H14V18H10Z";

        [Browsable(false)]
        public override string Name { get => base.Name; set => base.Name = value; }

        [Browsable(false)]
        public int X
        {
            get => _x;
            set => SetProperty(ref _x, value);
        }
        [Browsable(false)]
        public int Y
        {
            get => _y;
            set => SetProperty(ref _y, value);
        }
        [Browsable(false)]
        public bool Flow
        {
            get => _flow;
            set => SetProperty(ref _flow, value);
        }
        [Browsable(false)]
        public virtual bool Value { get; set; }

        [Browsable(false)]
        public bool VerticalLine
        {
            get => _verticalLine;
            set => SetProperty(ref _verticalLine, value);
        }
        public override void RemoveFromParent()
        {
            (this.Parent as IMachine).Logic.Remove(this);
            Extension.Unregister(this);
        }

    }
}
