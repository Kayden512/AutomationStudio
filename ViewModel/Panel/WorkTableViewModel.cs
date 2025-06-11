using Automation.PluginCore.Base;
using AutomationStudio.View;
using AutomationStudio.View.Panel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationStudio.ViewModel.Panel
{
    public class WorkTableViewModel : PanelBase
    {
        public override string Name => "WorkTable";
        public override Type ViewType => typeof(WorkTableView);
    }
}
