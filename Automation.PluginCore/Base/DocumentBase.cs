using Automation.PluginCore.Interface;
using Automation.PluginCore.Util;
using Automation.PluginCore.Util.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Automation.PluginCore.Base
{
    public abstract class DocumentBase : ViewModelBase, IDocument
    {
        INode _model;
        public INode Model
        {
            get => _model;
            set => SetProperty(ref _model, value);
        }
        public ICommand CmdClose => new RelayCommand(OnClose);

        public void OnClose()
        {
            Extension.CloseDocument(this);
        }
    }
}
