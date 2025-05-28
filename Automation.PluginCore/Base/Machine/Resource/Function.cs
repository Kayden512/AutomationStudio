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
    public class Function : LadderNodeBase, IReferenceHolder
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
                if (prevHolder != null)
                    prevHolder.ValueChanged -= Reference_ValueChanged;

                SetProperty(ref _referencePath, value);
                if (Guid.Empty.Equals(_reference) == false)
                {
                    this.Activate();
                }
            }
        }

        private void Reference_ValueChanged(object sender, object e)
        {
            this.Value = this.Option.Equals(e);
        }

        [JsonIgnore]
        [Browsable(false)]
        public IValueHolder Reference { get; set; }

        [PropertyOrder(1)]
        [Category("Reference")]
        [DisplayName("Option")]
        [RefreshProperties(RefreshProperties.All)]
        [Editor(typeof(ValueHolderOptionEditor), typeof(ValueHolderOptionEditor))]
        public object Option { get; set; }

        public override void Activate()
        {
            this.Reference = Extension.GetNodeById(ReferencePath) as IValueHolder;
            if (Reference != null)
            {
                if (Reference.Option != null && Option == null)
                {
                    var enumerator = Reference.Option.GetEnumerator();
                    if (enumerator.MoveNext())
                        this.Option = enumerator.Current;
                }
                Reference.ValueChanged += Reference_ValueChanged;
            }

            NotifyPropertyChanged(nameof(Reference));
            NotifyPropertyChanged(nameof(Option));

            base.Activate();
        }
    }
}
