using Automation.PluginCore.Base;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Automation.PluginCore.Interface
{
    public interface IDevice : INode
    {
        NodeCollection Actions { get; }
        List<Type> ActionMenu { get; }
        Task<object> ExecuteActionAsync(IAction action);
        void StopAction();
        void AddAction(INode action);
        void Connect();
        void Disconnect();
        void Initialize();
        void TryConnect();
        void TryDisconnect();
    }
}
