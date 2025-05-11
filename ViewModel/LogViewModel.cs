using Automation.PluginCore.Base;
using AutomationStudio.View;
using System;

namespace AutomationStudio.ViewModel
{
    public class LogViewModel : PanelBase
    {
        public override Type ViewType => typeof(LogView);
        public override string Name => "Log";
    }
}
