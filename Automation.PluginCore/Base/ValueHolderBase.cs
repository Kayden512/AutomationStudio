using Automation.PluginCore.Interface;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.PluginCore.Base
{
    public abstract class ValueHolderBase : NodeBase, IValueHolder
    {
        object _value;

        public event EventHandler<object> ValueChanged;

        [Browsable(false)]
        public virtual AccessMode AccessMode { get; set; } = AccessMode.ReadWrite;
        public object Value
        {
            get => _value;
            set
            {
                if (!EqualityComparer<object>.Default.Equals(_value, value))
                {
                    SetProperty(ref _value, value);
                    ValueChanged?.Invoke(this, value);
                    NotifyPropertyChanged(nameof(Icon));
                }
            }
        }
        public virtual ICollection Option => null;

    }
}
