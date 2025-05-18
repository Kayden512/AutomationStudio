using Automation.PluginCore.Base;
using Automation.PluginCore.Base.Device.PLC;
using Automation.PluginCore.CustomUI.View;
using Automation.PluginCore.CustomUI.ViewModel;
using Automation.PluginCore.Interface;
using Automation.PluginCore.Util;
using AutomationStudio.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AutomationStudio.ViewModel
{
    public class CustomScreenEditorViewModel : PanelBase
    {
        public override Type ViewType => typeof(CustomScreenEditorView);
        public override string Name => "UI Editor";

        public override void OnAppend(object param)
        {
            UITextBlock block = new UITextBlock();
            MapViewType(block);
            this.Items.Add(block);

        }
        public override void OnRemove(object param)
        {
            if (this.SelectedNode == null) return;
            if (this.Items.Contains(this.SelectedNode))
            {
                this.Items.Remove(this.SelectedNode);
                this.SelectedNode.Dispose();
                this.SelectedNode = null;
            }
            else
            {
                this.SelectedNode.Dispose();
                this.SelectedNode = null;
            }
        }
        static void MapViewType(IViewModel viewModel)
        {
            if (viewModel == null) return;
            var template = new DataTemplate
            {
                DataType = viewModel.GetType(),
                VisualTree = new FrameworkElementFactory(viewModel.ViewType)
            };
            var key = new DataTemplateKey(viewModel.GetType());
            if (Application.Current.Resources.Contains(key)) return;
            Application.Current.Resources.Add(key, template);
        }
    }
}
