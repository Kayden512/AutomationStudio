﻿using Automation.PluginCore.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.PluginCore.Interface
{
    public interface IMachine : IDevice, IGroup
    {
        bool IsRunning { get; }
        NodeCollection Schedules { get; }
        NodeCollection Logic { get; }

        void AddSchedule(INode schedule);
    }
}
