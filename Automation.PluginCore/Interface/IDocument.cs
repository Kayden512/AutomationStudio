﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Automation.PluginCore.Interface
{
    public interface IDocument : IViewModel
    {
        INode Model { get; }
        ICommand CmdClose { get; }
        void OnClose();
    }
}
