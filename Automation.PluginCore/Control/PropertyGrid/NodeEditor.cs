using Automation.PluginCore.Base;
using Automation.PluginCore.Base.Machine;
using Automation.PluginCore.Base.Machine.Resource;
using Automation.PluginCore.Interface;
using Automation.PluginCore.Util;
using Automation.PluginCore.Util.Behavior;
using Automation.PluginCore.Util.Converter;
using Automation.PluginCore.Util.Extension;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Automation.PluginCore.Control.PropertyGrid
{
    public class NodeEditor : ViewModelBase, ITypeEditor
    {
        PropertyItem Item { get; set; }
        public FrameworkElement ResolveEditor(PropertyItem propertyItem)
        {
            this.Item = propertyItem;

            ComboBox combo = new ComboBox();
            combo.DataContext = this;

            Type InstanceType = this.Item.Instance.GetType();
            PropertyInfo fieldInfo = InstanceType.GetProperty(this.Item.PropertyName);
            NodeTypeAttribute nta = (NodeTypeAttribute)fieldInfo.GetCustomAttribute(typeof(NodeTypeAttribute));
            if (nta == null) return combo;
            List<INode> nodes = Extension.GetNodes(nta.IncludedTypes);
            if(nta.ExcludedTypes != null)
            {
                nodes = nodes.Where(node => !nta.ExcludedTypes.Contains(node.GetType())).ToList();
            }
            List<string> paths = nodes.Select(a => a.Path).ToList();
            combo.ItemsSource = paths;

            var binding = new Binding(Item.PropertyName)
            {
                Source = Item.Instance,
                Mode = BindingMode.TwoWay,
                Converter = new PathToGuidConverter()
            };
            combo.SetBinding(ComboBox.SelectedItemProperty, binding);

            var behaviors = Interaction.GetBehaviors(combo);
            if(!behaviors.OfType<ItemsControlDragDropBehavior>().Any())
            {
                var behavior = new ItemsControlDragDropBehavior();
                Binding commandBinding = new Binding("CmdDrop") { Mode = BindingMode.OneWay };
                BindingOperations.SetBinding(behavior, ItemsControlDragDropBehavior.DropCommandProperty, commandBinding);
                behaviors.Add(behavior);
            }
            return combo;
        }

        public override void OnDrop(DropData drop)
        {
            if (drop.Source is IAction action)
                Item.Value = action.Id;
        }
    }
}
