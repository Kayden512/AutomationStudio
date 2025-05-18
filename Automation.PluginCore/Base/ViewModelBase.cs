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
        public virtual Type ViewType => null;

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
