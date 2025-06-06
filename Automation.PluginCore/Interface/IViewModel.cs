﻿using System;
using System.Windows;
using System.Windows.Input;

namespace Automation.PluginCore.Interface
{
    public interface IViewModel : INode
    {
        Type ViewType { get; }
        INode SelectedNode { get; set; }
        bool IsFocused { get; set; }
    }
}
