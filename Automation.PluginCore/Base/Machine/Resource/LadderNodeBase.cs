using Automation.PluginCore.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.PluginCore.Base.Machine.Resource
{
    
    public class LadderNode : NodeBase, ILadder
    {
        int _x;
        int _y;
        bool _flow;
        bool _value;
        bool _verticalLine;
        LadderType _ladderType = LadderType.None;

        public override string Icon => "M14 1V3H10V1H8V22H10V20H14V22H16V1H14M14 5V8H10V5H14M14 10V13H10V10H14M10 18V15H14V18H10Z";
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
        public bool Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
        public bool VerticalLine
        {
            get => _verticalLine;
            set => SetProperty(ref _verticalLine, value);
        }
        public LadderType Type
        {
            get => _ladderType;
            set => SetProperty(ref _ladderType, value);
        }
        public override void RemoveFromParent()
        {
            (this.Parent as IMachine).Logic.Remove(this);
        }
    }
}
