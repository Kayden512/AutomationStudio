using Automation.PluginCore.Interface;
using Automation.PluginCore.Util;
using System.Windows.Input;

namespace Automation.PluginCore.Base
{
    public abstract class PanelBase : ViewModelBase, IPanel
    {
        bool _isVisible = true;
        INode _selectedNode;

        #region Command
        public ICommand CmdAppend { get; set; }
        public ICommand CmdRemove { get; set; }
        public ICommand CmdSave { get; set; }
        #endregion

        #region CommandMethod
        public virtual void OnAppend(object param)
        {
        }
        public virtual void OnRemove(object param)
        {
        }
        public virtual void OnSave()
        {
        }
        #endregion

        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref this._isVisible, value);
        }
        public INode SelectedNode
        {
            get => _selectedNode;
            set => SetProperty(ref _selectedNode, value);
        }
    }
}
