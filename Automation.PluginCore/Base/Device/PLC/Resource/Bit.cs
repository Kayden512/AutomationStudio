using Automation.PluginCore.Interface;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Automation.PluginCore.Base.Device.PLC.Resource
{
    public class Bit : ValueHolderBase
    {
        public override string Icon
        {
            get
            {
                if(this.Value != null && this.Value.Equals(true))
                    return "M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12A10,10 0 0,0 12,2Z";
                return "M12,20A8,8 0 0,1 4,12A8,8 0 0,1 12,4A8,8 0 0,1 20,12A8,8 0 0,1 12,20M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12A10,10 0 0,0 12,2Z";
            }
        }

        [Browsable(false)]
        public override ICollection Option => new List<bool> { true, false };

        [JsonIgnore]
        [Browsable(false)]
        public virtual string DisplayAddress => this.Parent == null? "" : (this.Parent as Memory).Address + "." +(this.Parent as Memory).Items.IndexOf(this);
        public Bit() : base()
        {
            this.Value = false;
        }
    }
}
