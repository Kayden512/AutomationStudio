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
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Automation.PluginCore.Control.PropertyGrid
{
    public class ValueHolderOptionEditor : ViewModelBase, ITypeEditor
    {
        PropertyItem Item { get; set; }
        public FrameworkElement ResolveEditor(PropertyItem propertyItem)
        {
            this.Item = propertyItem;
            ComboBox combo = new ComboBox();

            combo.DataContext = this;
            IValueHolder valueHolder = (Item.Instance as LadderNode).Reference;
            combo.ItemsSource = null;
            if (valueHolder != null )
                combo.ItemsSource = valueHolder.Option;

            var binding = new Binding(Item.PropertyName)
            {
                Source = Item.Instance,
                Mode = BindingMode.TwoWay,
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
    }
}
