using Automation.PluginCore.Interface;
using System.Windows;
using System.Windows.Controls;

namespace AutomationStudio.Util
{
    public class PaneStyleSelector: StyleSelector
    {
        public Style Document { get; set; }
        public Style Anchorable { get; set; }
        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is IDocument)
                return Document;
            else
                return Anchorable;
        }
    }
}
