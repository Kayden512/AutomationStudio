using Automation.PluginCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.PluginCore.Base.Machine.Resource
{
    public interface ILadder : INode
    {
        int X { get; set; }
        int Y { get; set; }
        bool Flow { get; set; }
        bool Value { get; set; }
        bool VerticalLine { get; set; }
    }

    public interface IReferenceHolder
    {
        IValueHolder Reference { get; set; }
    }
}
