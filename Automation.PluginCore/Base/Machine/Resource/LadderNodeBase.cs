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

        public override string Icon => "M 0,10 L 20,10 M 20,0 L 20,20 M 40,0 L 40,20 M 40,10 L 60,10";

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
        public bool Monitor { get; set; }

        public bool Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
    }
}
