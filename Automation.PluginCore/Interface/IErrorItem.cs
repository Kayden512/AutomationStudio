using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Automation.PluginCore.Interface
{
    public enum ErrorSeverity
    {
        Error = 0,
        Warning = 1,
        Info = 2
    }
    public interface IErrorItem
    {
        ErrorSeverity Severity { get; }
        string Code { get;}
        string Message { get;}
        string Node { get;}
        string Path { get;}
    }
}
