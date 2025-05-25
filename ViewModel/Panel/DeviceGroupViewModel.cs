using Automation.PluginCore.Base;
using Automation.PluginCore.Base.Device.PLC;
using Automation.PluginCore.Interface;
using Automation.PluginCore.Util;
using Automation.PluginCore.Util.Behavior;
using Automation.PluginCore.Util.Extension;
using AutomationStudio.View;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AutomationStudio.ViewModel
{
    public class DeviceGroupViewModel : PanelBase
    {
        public override Type ViewType => typeof(DeviceGroupView);
        public override string Name => "Device";

        public ObservableCollection<Type> Menu { get; set; } = new ObservableCollection<Type>();

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
            this.AddChild(newNode);
        }
        public override void OnRemove(object param)
        {
            if (this.SelectedNode == null) return;
            SelectedNode.RemoveFromParent();
            if (this.Items.Count != 0)
                this.SelectedNode = this.Items[0];
        }
        public override void OnDrop(DropData drop)
        {
            if (drop.Source != null && drop.Target != null && drop.Source != drop.Target)
            {
                if (!(drop.Source is IDevice)) return;
                if (drop.Source is IViewModel) return;

                if (drop.Source.Items.Contains(drop.Target)) return;

                //Group이 아니면 Return
                if ((drop.Target is IGroup) == false) return;

                drop.Target.AddChild(drop.Source);
            }
        }
        public override void OnSave()
        {
            Extension.SaveToJson<NodeCollection>("Environment\\" + this.Name + ".json", this.Items);
        }
        public void LoadData()
        {
            NodeCollection loaded = Extension.LoadFromJson<NodeCollection>("Environment\\" + this.Name + ".json");
            if(loaded != null)
            {
                foreach (INode node in loaded)
                {
                    this.Items.Add(node);
                }
            }
        }

    }
}
