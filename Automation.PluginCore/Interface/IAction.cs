using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Automation.PluginCore.Interface
{
    public enum ActionStatus { None, Running, Stopped, Error, Complete }
    public interface IAction : INode
    {
        bool IsEnabled { get; }
        ActionStatus ActionStatus { get; set; }
        Task<object> ExecuteAsync(CancellationToken token);
    }
}
