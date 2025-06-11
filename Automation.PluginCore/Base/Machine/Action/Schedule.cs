using Automation.PluginCore.Interface;
using Automation.PluginCore.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Automation.PluginCore.Base.Machine
{
    public class Schedule : ActionBase, IGroup
    {
        public override string Icon => "M19,19V8H5V19H19M16,1H18V3H19A2,2 0 0,1 21,5V19A2,2 0 0,1 19,21H5C3.89,21 3,20.1 3,19V5C3,3.89 3.89,3 5,3H6V1H8V3H16V1M7,10H9V12H7V10M15,10H17V12H15V10M11,14H13V16H11V14M15,14H17V16H15V14Z";

        public override async Task<object> ExecuteAsync(CancellationToken token)
        {
            try
            {
                foreach (IAction action in Items)
                {
                    await (this.Parent as IMachine).ExecuteActionAsync(action);
                }
                return true;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        public override void RemoveFromParent()
        {
            (Parent as IMachine)?.Schedules.Remove(this);
        }

        public override IEnumerable<IErrorItem> Validate()
        {
            if (this.Items.Count == 0)
            {
                yield return new ErrorItem
                {
                    Severity = ErrorSeverity.Warning,
                    Code = "NODE010",
                    Message = "Schedule has no action",
                    Node = this.Name,
                    Path = this.Path
                };
            }
            foreach (IErrorItem item in base.Validate())
            {
                yield return item;
            }
        }
    }
}
