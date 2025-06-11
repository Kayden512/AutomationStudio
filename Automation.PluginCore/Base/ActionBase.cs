using Automation.PluginCore.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Automation.PluginCore.Base
{
    [CategoryOrder("Result", 99)]
    public abstract class ActionBase : NodeBase, IAction
    {
        bool _isEnabled = true;
        ActionStatus _actionStatus;

        public override string Icon => "M10,4H4C2.89,4 2,4.89 2,6V18A2,2 0 0,0 4,20H20A2,2 0 0,0 22,18V8C22,6.89 21.1,6 20,6H12L10,4Z";

        [JsonIgnore]
        [Browsable(false)]
        protected IDevice Device => Parent == null ? null : Parent as IDevice;

        [Browsable(false)]
        public ActionStatus ActionStatus
        {
            get => _actionStatus;
            set => SetProperty(ref _actionStatus, value);
        }

        [Category("Base")]
        public bool IsEnabled  
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }
        public override void RemoveFromParent()
        {
            Device?.Actions.Remove(this);
        }
        public virtual async Task<object> ExecuteAsync(CancellationToken token)
        {
            await Task.Delay(0);
            return false;
        }
    }
}
