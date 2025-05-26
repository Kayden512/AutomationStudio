using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Automation.PluginCore.Interface
{
    //public interface IValueHolder<T> : INode, IValueHolder
    //{
    //    T Value { get; set; }
    //}

    public interface IValueHolder
    {
        event EventHandler<object> ValueChanged;

        ICollection Option { get; }

        object Value { get; set; }
    }
}
