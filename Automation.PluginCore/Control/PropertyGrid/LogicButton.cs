using Automation.PluginCore.Base.Device.PLC.ViewModel;
using Automation.PluginCore.Base.Machine.ViewModel;
using Automation.PluginCore.Interface;
using Automation.PluginCore.Util.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Automation.PluginCore.Control.PropertyGrid
{
    public class LogicButton : ITypeEditor
    {
        LadderViewModel LadderViewModel { get; set; } = new LadderViewModel();
        public FrameworkElement ResolveEditor(PropertyItem propertyItem)
        {
            LadderViewModel.Model = propertyItem.Instance as INode;
            Button btn = new Button();
            btn.Content = "Open";
            btn.Click += Btn_Click;
            return btn;
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Extension.OpenDocument(LadderViewModel);
        }
    }
}
