using Automation.PluginCore.Control.PropertyGrid;
using Automation.PluginCore.Interface;
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
    public class Coil : LadderNodeBase, IReferenceHolder
    {
        bool _value;
        Guid _referencePath;
        IValueHolder _reference;
        public override string Icon => "M 0,10 L 20,10 M 25,0 C 20,0 20,20 25,20 M 35,20 C 40,20 40,0 35,0 M 40,10 L 60,10";

        public override bool Value
        {
            get => _value;
            set
            {
                SetProperty(ref _value, value);
                if(Reference != null)
                    Reference.Value = value;
            }
        }

        [PropertyOrder(0)]
        [Category("Reference")]
        [DisplayName("Reference")]
        [RefreshProperties(RefreshProperties.All)]
        [Editor(typeof(WriteableValueHolderEditor), typeof(WriteableValueHolderEditor))]
        public Guid ReferencePath
        {
            get => _referencePath;
            set
            {
                IValueHolder prevHolder = Extension.GetNodeById(_referencePath) as IValueHolder;
                SetProperty(ref _referencePath, value);
                if (Guid.Empty.Equals(_reference) == false)
                    this.Activate();
            }
        }

        [JsonIgnore]
        [Browsable(false)]
        public IValueHolder Reference { get; set; }

        public override void Activate()
        {
            this.Reference = Extension.GetNodeById(ReferencePath) as IValueHolder;
            NotifyPropertyChanged(nameof(Reference));
            base.Activate();
        }
    }
}
