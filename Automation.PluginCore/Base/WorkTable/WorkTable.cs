using Automation.PluginCore.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.PluginCore.Base.WorkTable
{
    public class WorkTable : ValueHolderBase
    {
        public override string Icon => "M2 7H22V10H20L21 19H18.5L17.94 14H6.06L5.5 19H3L4 10H2V7M17.5 10H6.5L6.29 12H17.71L17.5 10Z";
        public override AccessMode AccessMode => AccessMode.ReadWrite;

        public ObservableCollection<string> States { get; set; } = new ObservableCollection<string>();

        public override ICollection Option => States;
    }
}
