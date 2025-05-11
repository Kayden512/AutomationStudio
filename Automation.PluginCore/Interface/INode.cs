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
        string DisplayImage { get; }
        string Name { get; }
        Guid guid { get; }
        bool IsSelected { get; }

        #region Tree
        string Path { get; }
        INode Parent { get; set; }
        NodeCollection Items { get; set; }
        INode FindNode(string path);
        void RemoveFromParent();
        void AddChild(INode child);
        #endregion

        void Activate();
    }
}
