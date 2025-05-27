using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Automation.PluginCore.Interface
{
    [Flags]
    public enum AccessMode
    {
        Read = 1,
        Write = 2,
        ReadWrite = Read | Write
    }
    public interface IValueHolder
    {
        event EventHandler<object> ValueChanged;

        AccessMode AccessMode { get; set; }
        ICollection Option { get; }

        object Value { get; set; }
    }
}
