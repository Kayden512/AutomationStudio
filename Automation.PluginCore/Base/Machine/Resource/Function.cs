using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.PluginCore.Base.Machine.Resource
{
    public class Function : LadderNodeBase
    {
        bool _value;
        public override string Icon => "M 0,10 L 20,10 M 25,0 L 20,0 L 20,20 L 25,20 M 35,0 L 40,0 L 40,20 L 35,20 M 40,10 L 60,10";
        public override bool Value
        {
            get => _value;
            set
            {
                SetProperty(ref _value, value);
            }
        }
    }
}
