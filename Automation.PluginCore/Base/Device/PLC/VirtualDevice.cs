using Automation.PluginCore.Interface;
using Automation.PluginCore.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.PluginCore.Base.Device.PLC
{
    public class VirtualDevice : DeviceBase
    {
        public override string Icon => "M17,17H7V7H17M21,11V9H19V7C19,5.89 18.1,5 17,5H15V3H13V5H11V3H9V5H7C5.89,5 5,5.89 5,7V9H3V11H5V13H3V15H5V17A2,2 0 0,0 7,19H9V21H11V19H13V21H15V19H17A2,2 0 0,0 19,17V15H21V13H19V11M13,13H11V11H13M15,9H9V15H15V9Z";

        //ObservableCollection<INode> Memory

        public override IEnumerable<IErrorItem> Validate()
        {
            yield return new ErrorItem
            {
                Severity = ErrorSeverity.Warning,
                Code = "NODE000",
                Message = "test.",
                Node = this.Name,
                Path = this.Path
            };
            foreach (IErrorItem item in base.Validate()) 
            {
                yield return item;
            }
        }

    }
}
