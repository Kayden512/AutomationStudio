using Automation.PluginCore;
using Automation.PluginCore.Base;
using Automation.PluginCore.Base.Machine;
using Automation.PluginCore.Base.Machine.ViewModel;
using Automation.PluginCore.Interface;
using Automation.PluginCore.Util;
using Automation.PluginCore.Util.Behavior;
using AutomationStudio.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AutomationStudio.ViewModel
{
    public class ScheduleViewModel : PanelBase
    {
        IMachine _machine;

        public override string Name => "Schedule";
        public override Type ViewType => typeof(ScheduleView);
        LadderViewModel LogicEditor = new LadderViewModel();


        public ICommand CmdOpenEditor => new RelayCommand(OpenEditor);

        public IMachine Machine
        {
            get => _machine;
            set => SetProperty(ref _machine, value);
        }
        public void OpenEditor()
        {
            LogicEditor.Model = this;
            Access.Instance.OpenDocument(LogicEditor);
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
            if (newNode is Schedule)
                Machine.Schedules.Add(newNode);
            else
            {
                if (this.SelectedNode is Schedule)
                    SelectedNode.AddChild(newNode);
                else
                    SelectedNode.Parent.AddChild(newNode);
            }
        }
        public override void OnRemove(object param)
        {
            if (this.SelectedNode == null) return;
            this.SelectedNode.RemoveFromParent();
            if (Machine.Schedules.Count != 0)
                this.SelectedNode = Machine.Schedules[0];
        }
        public override void OnDrop(DropData drop)
        {
            if ((drop.Source is IAction action) == false) return;
            if (drop.Source is Schedule) return;
            if (drop.Source is Request)
            {
                if (drop.Target is Schedule schedule)
                    schedule.AddChild(drop.Source);
            }
            else
            {
                Request request = new Request()
                {
                    Name = drop.Source.Name,
                    Action = drop.Source as IAction
                };
                if (drop.Target is Schedule schedule)
                {
                    schedule.AddChild(request);
                }
                else if (drop.Target is Request targetRequest)
                {
                    int index = targetRequest.Parent.Items.IndexOf(targetRequest);
                    targetRequest.Parent.Items.Insert(index, request);
                }
            }
        }
    }
}
