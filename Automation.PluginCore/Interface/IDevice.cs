using Automation.PluginCore.Base;
using System;
using System.Collections.Generic;

namespace Automation.PluginCore.Interface
{
    public interface IDevice : INode
    {
        NodeCollection Actions { get; }

        List<Type> ActionMenu { get; }

        void Connect();
        void Disconnect();
        void Initialize();
    }
}
