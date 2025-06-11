using Automation.PluginCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Automation.PluginCore.Base.Device.PLC.Action
{
    public class WriteBit : ActionBase
    {
        int _address;
        int _bit;
        bool _value;

        public int Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }
        public int Bit
        {
            get => _bit;
            set => SetProperty(ref _bit, value);
        }
        public bool Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
        public override async Task<object> ExecuteAsync(CancellationToken token)
        {
            VirtualDevice device = this.Parent as VirtualDevice;
            if (device == null) throw new InvalidOperationException("Device Null");
            (device.Input[Address].Items[Bit] as IValueHolder).Value = Value;
            return true;
        }
    }
}
