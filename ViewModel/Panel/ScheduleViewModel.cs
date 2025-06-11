using Automation.PluginCore;
using Automation.PluginCore.Base;
using Automation.PluginCore.Base.Machine;
using Automation.PluginCore.Base.Machine.ViewModel;
using Automation.PluginCore.Interface;
using Automation.PluginCore.Util;
using Automation.PluginCore.Util.Behavior;
using Automation.PluginCore.Util.Extension;
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
            set
            {
                SetProperty(ref _machine, value);
                LogicEditor.Model = Machine;
            }
        }

        #region Command

        public ICommand CmdExecute => new RelayCommand(OnExecute);
        public ICommand CmdStop => new RelayCommand(OnStop);
        public ICommand CmdErase => new RelayCommand(OnErase);

        #endregion
        public async void OnExecute()
        {
            if (_machine == null) return;
            if (this.SelectedNode == null) return;

            try
            {
                if (this.SelectedNode is Schedule schedule)
                {
                    if (_machine.Schedules.Contains(this.SelectedNode) == false) return;
                    await _machine.ExecuteActionAsync(this.SelectedNode as IAction);
                }
                else
                {
                    await _machine.ExecuteActionAsync(this.SelectedNode as IAction);
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void OnStop()
        {
            _machine.StopAction();
        }
        public void OnErase()
        {
            if(this.SelectedNode == null) return;

            if(this.SelectedNode is Schedule schedule)
            {
                foreach(IAction action in schedule.Items)
                {
                    action.ActionStatus = ActionStatus.None;
                }
                schedule.ActionStatus = ActionStatus.None;
            }
            else
            {
                (this.SelectedNode as IAction).ActionStatus = ActionStatus.None;
            }
        }


        public void OpenEditor()
        {
            if (Machine == null) return;
            LogicEditor.Model = Machine;
            Extension.OpenDocument(LogicEditor);
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
                Machine.AddSchedule(newNode);
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
                    ActionPath = drop.Source.Id,
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
