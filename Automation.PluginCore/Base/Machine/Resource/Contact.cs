using Automation.PluginCore.Control.PropertyGrid;
using Automation.PluginCore.Interface;
using Automation.PluginCore.Util.Extension;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Automation.PluginCore.Base.Machine.Resource
{
    public enum ContactType { A, B }
    [CategoryOrder("Contact",2)]
    public class Contact : LadderNodeBase, IReferenceHolder
    {
        bool _value;
        Guid _referencePath;
        IValueHolder _reference;
        ContactType _contactType;

        public override string Icon
        {
            get
            {
                if (this.Type == ContactType.A) return "M 0,10 L 20,10 M 20,0 L 20,20 M 40,0 L 40,20 M 40,10 L 60,10";
                return "M 0,10 L 20,10 M 20,0 L 20,20 M 40,0 L 40,20 M 40,10 L 60,10 M 17,2 L 43,18";
            }
        }
        [PropertyOrder(0)]
        [Category("Contact")]
        public ContactType Type
        {
            get => _contactType;
            set
            {
                SetProperty(ref _contactType, value);
                NotifyPropertyChanged(nameof(Icon));
            }
        }

        public override bool Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
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
