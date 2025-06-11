using Automation.PluginCore.Base;
using Automation.PluginCore.Base.WorkTable;
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
        public override void OnSelect(object param)
        {
            SelectedNode = null;
            if (param is WorkTable node)
                SelectedNode = node;
        }

        public WorkTableViewModel() 
        {
            this.Items.Add(new WorkTable());
        }
    }
}
