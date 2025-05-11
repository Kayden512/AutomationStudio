using Automation.PluginCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Automation.PluginCore.Base
{
    public abstract class PanelBase : ViewModelBase, IPanel
    {
        #region Command
        public ICommand CmdAppend { get; set; }
        public ICommand CmdRemove { get; set; }
        public ICommand CmdSave { get; set; }

        #endregion
        bool _isVisible = true;
        public bool IsVisible
        {
            get => this._isVisible;
            set => SetProperty(ref this._isVisible, value);
        }
        INode _selectedNode;
        public INode SelectedNode
        {
            get => _selectedNode;
            set => SetProperty(ref _selectedNode, value);
        }
    }
}
