using Automation.PluginCore.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.PluginCore.Interface
{
    public interface INode : INotifyPropertyChanged, IDisposable
    {
        string Icon { get; }
        string Name { get; }
        Guid Id { get; }
        bool IsSelected { get; }

        #region Tree
        string Path { get; }
        NodeCollection Items { get; }
        INode Parent { get; set; }
        INode FindNode(string path);
        void RemoveFromParent();
        void AddChild(INode node);
        #endregion

        IEnumerable<IErrorItem> Validate();
        IEnumerable<IErrorItem> CollectErrors();

        void Activate();
    }
}
