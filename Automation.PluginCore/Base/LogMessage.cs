using Automation.PluginCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.PluginCore.Base
{
    public class LogMessage : NodeBase
    {
        string _message;
        ErrorSeverity _level;

        public ErrorSeverity Level
        {
            get => _level;
            set => SetProperty(ref _level, value);
        }
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public LogMessage(ErrorSeverity level, string message)
        {
            Level = level; Message = message;
        }

    }
}
