using Automation.PluginCore.Control.PropertyGrid;
using Automation.PluginCore.Interface;
using Automation.PluginCore.Util.Extension;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Automation.PluginCore.Base.Machine.Resource
{
    [CategoryOrder("Ladder",2)]
    public class LadderNode : NodeBase, ILadder
    {
        int _x;
        int _y;
        bool _flow;
        bool _value;
        bool _verticalLine;
        LadderType _ladderType = LadderType.None;
        Guid _referencePath;
        IValueHolder _reference;

        public override string Icon => "M14 1V3H10V1H8V22H10V20H14V22H16V1H14M14 5V8H10V5H14M14 10V13H10V10H14M10 18V15H14V18H10Z";

        [Browsable(false)]
        public override string Name { get => base.Name; set => base.Name = value; }

        [Browsable(false)]
        public int X
        {
            get => _x;
            set => SetProperty(ref _x, value);
        }
        [Browsable(false)]
        public int Y
        {
            get => _y;
            set => SetProperty(ref _y, value);
        }
        [Browsable(false)]
        public bool Flow
        {
            get => _flow;
            set => SetProperty(ref _flow, value);
        }
        [Browsable(false)]
        public bool Value
        {
            get => _value;
            set
            {
                SetProperty(ref _value, value);
                if(this.Type == LadderType.Coil && this.Reference != null)
                    this.Reference.Value = value;
                else if (this.Type == LadderType.Function && this.Reference != null && this.Option != null)
                    this.Reference.Value = this.Option;
            }
        }
        [Browsable(false)]
        public bool VerticalLine
        {
            get => _verticalLine;
            set => SetProperty(ref _verticalLine, value);
        }
        [Browsable(false)]
        public LadderType Type
        {
            get => _ladderType;
            set
            {
                SetProperty(ref _ladderType, value);
                NotifyPropertyChanged(nameof(Reference));
                NotifyPropertyChanged(nameof(Option));
            }
        }
        public override void RemoveFromParent()
        {
            (this.Parent as IMachine).Logic.Remove(this);
            Extension.Unregister(this);
        }

        [PropertyOrder(0)]
        [Category("Reference")]
        [DisplayName("Reference")]
        [RefreshProperties(RefreshProperties.All)]
        [Editor(typeof(ValueHolderEditor), typeof(ValueHolderEditor))]
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
            if(Reference != null)
            {
                if (Reference.Option != null && Option == null)
                {
                    var enumerator = Reference.Option.GetEnumerator();
                    if(enumerator.MoveNext())
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
