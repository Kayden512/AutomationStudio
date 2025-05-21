using Automation.PluginCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using Automation.PluginCore.Util;
using Automation.PluginCore.Util.Behavior;

namespace Automation.PluginCore.Base
{
    public abstract class ViewModelBase : NodeBase, IViewModel
    {
        bool _isFocused;
        INode _selectedNode;
        public virtual Type ViewType => null;

        public bool IsFocused
        {
            get => _isFocused;
            set => SetProperty(ref _isFocused, value);
        }

        public INode SelectedNode
        {
            get => _selectedNode;
            set => SetProperty(ref _selectedNode, value);
        }

        public ICommand CmdDrop => new RelayCommand<DropData>(OnDrop);

        public virtual void OnDrop(DropData drop)
        {
            if (drop.Source != null && drop.Target != null && drop.Source != drop.Target)
            {
                if (drop.Source is IViewModel) return;

                if (drop.Source.Items.Contains(drop.Target)) return;
                
                //Group이 아니면 Return
                if ((drop.Target is IGroup) == false) return;

                drop.Target.AddChild(drop.Source);
            }
        }
    }
}
