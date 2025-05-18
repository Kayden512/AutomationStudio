using Automation.PluginCore.Base;
using Automation.PluginCore.Interface;
using AutomationStudio.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationStudio.ViewModel
{
    public class PropertyEditorViewModel : PanelBase
    {
        object _selectedObject;

        public override Type ViewType => typeof(PropertyEditorView);
        public override string Name => "Property";

        public object SelectedObject
        {
            get => _selectedObject;
            set => SetProperty(ref _selectedObject, value);
        }
    }
}
