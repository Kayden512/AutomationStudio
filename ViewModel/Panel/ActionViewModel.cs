using Automation.PluginCore.Base;
using Automation.PluginCore.Interface;
using AutomationStudio.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using System.Runtime.CompilerServices;
using Automation.PluginCore.Util;

namespace AutomationStudio.ViewModel
{
    public class ActionViewModel : PanelBase
    {
        IDevice _device;

        public override Type ViewType => typeof(ActionView);
        public override string Name => "Action";

        public IDevice Device
        {
            get => _device;
            set => SetProperty(ref _device, value);
        }

        public override void OnSelect(object param)
        {
            SelectedNode = null;
            if (param is NodeBase node)
                SelectedNode = node;
        }
        public override void OnAppend(object param)
        {
            if (param == null) return;
            var obj = Activator.CreateInstance(param as Type);
            INode newNode = obj as INode;
            Device.AddAction(newNode as IAction);
        }
        public override void OnRemove(object param)
        {
            if (this.SelectedNode == null) return;
            this.SelectedNode.RemoveFromParent();
            if (Device.Actions.Count != 0)
                this.SelectedNode = Device.Actions[0];
        }
    }
}
