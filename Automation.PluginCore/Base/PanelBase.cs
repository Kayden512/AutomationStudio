using Automation.PluginCore.Interface;
using Automation.PluginCore.Util;
using System.Windows.Input;

namespace Automation.PluginCore.Base
{
    public abstract class PanelBase : ViewModelBase, IPanel, IGroup
    {
        bool _isVisible = true;

        #region Command
        public ICommand CmdAppend => new RelayCommand<object>(OnAppend);
        public ICommand CmdRemove => new RelayCommand<object>(OnRemove);
        public ICommand CmdSave => new RelayCommand(OnSave);
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
    }
}
