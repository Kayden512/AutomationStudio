using Automation.PluginCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.PluginCore.Util
{
    public class ErrorItem : IErrorItem
    {
        public ErrorSeverity Severity { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public string Node { get; set; }
        public string Path { get; set; }
    }
}
