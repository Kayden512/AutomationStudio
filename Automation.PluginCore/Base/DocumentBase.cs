using Automation.PluginCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
