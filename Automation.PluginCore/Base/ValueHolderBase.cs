using Automation.PluginCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.PluginCore.Base
{
    public abstract class ValueHolderBase<T> : NodeBase, IValueHolder<T>
    {
        public event EventHandler ValueChanged;

        T _value;
        public T Value
        {
            get => _value;
            set
            {
                if (!EqualityComparer<T>.Default.Equals(_value, value))
                {
                    SetProperty(ref _value, value);
                    ValueChanged?.Invoke(this, new EventArgs());
                    NotifyPropertyChanged(nameof(Icon));
                }
            }
        }
    }
}
