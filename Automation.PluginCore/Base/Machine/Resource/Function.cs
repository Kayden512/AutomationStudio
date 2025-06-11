using Automation.PluginCore.Control.PropertyGrid;
using Automation.PluginCore.Interface;
using Automation.PluginCore.Util;
using Automation.PluginCore.Util.Extension;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Automation.PluginCore.Base.Machine.Resource
{
    public class Function : LadderNodeBase
    {
        bool _value;
        Guid _referencePath;
        IValueHolder _reference;
        public override string Icon => "M 0,10 L 20,10 M 25,0 L 20,0 L 20,20 L 25,20 M 35,0 L 40,0 L 40,20 L 35,20 M 40,10 L 60,10";
        public override bool Value
        {
            get => _value;
            set
            {
                SetProperty(ref _value, value);
                if(value == true && this.Reference != null)
                {
                    (Reference.Parent as IMachine).ExecuteActionAsync(this.Reference);
                }
            }
        }

        [PropertyOrder(0)]
        [Category("Reference")]
        [DisplayName("Reference")]
        [RefreshProperties(RefreshProperties.All)]
        [NodeTypeAttribute(typeof(Schedule))]
        [Editor(typeof(NodeEditor), typeof(NodeEditor))]
        public Guid ReferencePath
        {
            get => _referencePath;
            set
            {
                SetProperty(ref _referencePath, value);
                if (Guid.Empty.Equals(_reference) == false)
                    this.Activate();
            }
        }

        [JsonIgnore]
        [Browsable(false)]
        public Schedule Reference { get; set; }

        public override void Activate()
        {
            this.Reference = Extension.GetNodeById(ReferencePath) as Schedule;
            NotifyPropertyChanged(nameof(Reference));
            base.Activate();
        }
    }
}
