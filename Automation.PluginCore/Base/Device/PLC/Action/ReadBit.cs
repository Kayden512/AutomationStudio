using Automation.PluginCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Automation.PluginCore.Base.Device.PLC.Action
{
    public class ReadBit : ActionBase
    {
        int _address;
        int _bit;

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

        public override async Task<object> ExecuteAsync(CancellationToken token)
        {
            VirtualDevice device = this.Parent as VirtualDevice;
            if (device == null) throw new InvalidOperationException("Device Null");
            return ((device.Input[Address].Items[Bit]) as IValueHolder).Value;
        }
    }
}
