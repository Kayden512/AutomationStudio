using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.PluginCore.Base.Machine.Resource
{
    
    public enum LadderType { None, Contact_A, Contact_B , HorizontalLine, Coil }
    public class LadderNode : NodeBase, ILadder
    {
        int _x;
        int _y;
        LadderType _ladderType = LadderType.None;
        bool _verticalLine;
        bool _value;
        bool _flow;

        public override string Icon => "M14 1V3H10V1H8V22H10V20H14V22H16V1H14M14 5V8H10V5H14M14 10V13H10V10H14M10 18V15H14V18H10Z";

        public int X
        {
            get => _x;
            set => SetProperty(ref _x, value);
        }
        public int Y
        {
            get => _y;
            set => SetProperty(ref _y, value);
        }
        public LadderType Type
        {
            get => _ladderType;
            set => SetProperty(ref _ladderType, value);
        }

        public bool VerticalLine
        {
            get => _verticalLine;
            set => SetProperty(ref _verticalLine, value);
        }
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
    }
}
