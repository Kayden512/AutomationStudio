using Automation.PluginCore.Base;
using Automation.PluginCore.Base.Device.PLC;
using Automation.PluginCore.Interface;
using Automation.PluginCore.Util;
using AutomationStudio.View;
using System;
using System.Diagnostics;
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

        #region Commands
        public ICommand CmdSelect { get; }
        #endregion


        public void MouseDown(object sender, MouseButtonEventArgs e)
        {
            TreeView tree = sender as TreeView;
            if (tree.SelectedItem != null)
            {
                this.SelectedNode = tree.SelectedItem as INode;
            }
        }
        public void SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selected = e.NewValue;
            this.SelectedNode = selected as INode;
        }
        void OnAppend(object param)
        {
            if (this.Items == null) this.Items = new NodeCollection();
            this.Items.Add(new TestTool() { Parent = this });
        }
        void OnRemove(object param)
        {
            if (this.SelectedNode == null) return;
            if (this.Items.Contains(this.SelectedNode))
            {
                this.SelectedNode.Dispose();
                this.Items.Remove(this.SelectedNode);
                this.SelectedNode = null;
            }
            else
            {
                this.SelectedNode.Dispose();
                this.SelectedNode = null;
            }
            if (this.Items.Count != 0)
                this.SelectedNode = this.Items[0];
        }
        void OnSave()
        {
            Extension.SaveToJson<NodeCollection>("test.json", this.Items);   
        }

        public DeviceGroupViewModel()
        {
            CmdAppend = new RelayCommand<object>(OnAppend);
            CmdRemove = new RelayCommand<object>(OnRemove);
            CmdSave = new RelayCommand(OnSave);
        }
    }
}
