using Automation.PluginCore.Base;
using AutomationStudio.View;
using System;

namespace AutomationStudio.ViewModel
{
    /// <summary>
    /// Device 동작 로그 ViewModel
    /// </summary>
    public class LogViewModel : PanelBase
    {
        public override Type ViewType => typeof(LogView);
        public override string Name => "Log";
    }
}
