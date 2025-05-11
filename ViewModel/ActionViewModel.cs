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

namespace AutomationStudio.ViewModel
{
    public class ActionViewModel : PanelBase
    {
        public override Type ViewType => typeof(ActionView);
        public override string Name => "Action";

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
    }
}
