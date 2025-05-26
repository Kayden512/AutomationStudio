using Automation.PluginCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.PluginCore.Base.Machine.Resource
{
    public enum LadderType { None, Contact_A, Contact_B, HorizontalLine, Coil, Function }
    public interface ILadder : INode
    {
        int X { get; set; }
        int Y { get; set; }

        LadderType Type { get; set; }
        bool VerticalLine { get; set; }

        bool Flow { get; set; }

        bool Value { get; set; }
    }
}
