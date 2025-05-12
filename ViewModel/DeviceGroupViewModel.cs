using Automation.PluginCore.Base;
using Automation.PluginCore.Base.Device.PLC;
using Automation.PluginCore.Interface;
using Automation.PluginCore.Util;
using AutomationStudio.View;
using System;
using System.Collections.ObjectModel;
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

        public ObservableCollection<Type> Menu { get; set; } = new ObservableCollection<Type>();

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
        public override void OnAppend(object param)
        {
            if (param == null) return;
            var aa = Activator.CreateInstance(param as Type);
            IDevice newNode = aa as IDevice;

            this.Items.Add(newNode as INode);
        }
        public override void OnRemove(object param)
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
        public override void OnSave()
        {
            Extension.SaveToJson<NodeCollection>("Environment\\" + this.Name + ".json", this.Items);
        }
        public void LoadData()
        {
            this.Items = Extension.LoadFromJson<NodeCollection>("Environment\\" + this.Name + ".json");
        }

        public DeviceGroupViewModel()
        {
            CmdAppend = new RelayCommand<object>(OnAppend);
            CmdRemove = new RelayCommand<object>(OnRemove);
            CmdSave = new RelayCommand(OnSave);
        }
    }
}
