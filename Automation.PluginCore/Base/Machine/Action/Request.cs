using Automation.PluginCore.Control.PropertyGrid;
using Automation.PluginCore.Interface;
using Automation.PluginCore.Util;
using Automation.PluginCore.Util.Extension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Automation.PluginCore.Base.Machine
{
    [CategoryOrder("Action",10)]
    public class Request : ActionBase
    {
        NodeCollection _variableRef;
        Guid _actionPath;
        public override string Icon => "M10 21H14C14 22.1 13.1 23 12 23S10 22.1 10 21M21 19V20H3V19L5 17V11C5 7.9 7 5.2 10 4.3V4C10 2.9 10.9 2 12 2S14 2.9 14 4V4.3C17 5.2 19 7.9 19 11V17L21 19M17 11C17 8.2 14.8 6 12 6S7 8.2 7 11V18H17V11Z";
        
        [PropertyOrder(0)]
        [Category("Action")]
        public NodeCollection VariableRef { get; set; } = new NodeCollection();

        [JsonIgnore]
        [Browsable(false)]
        public IAction Action => Extension.GetNodeById(ActionPath) as IAction;

        [Category("Action")]
        [DisplayName("Action")]
        [Editor(typeof(RequestActionEditor), typeof(RequestActionEditor))]
        public Guid ActionPath
        {
            get => _actionPath;
            set
            {
                SetProperty(ref _actionPath, value);
                NotifyPropertyChanged(nameof(Action));
            }
        }

        public override void RemoveFromParent()
        {
            if (this.Device == null)
            {
                if (Parent is Schedule)
                    Parent.Items.Remove(this);
            }
            else
                base.RemoveFromParent();
        }

        public Request() : base()
        {
            this.VariableRef.CollectionChanged += Items_CollectionChanged;
        }

    }
}
