using Automation.PluginCore.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Automation.PluginCore.Base
{
    /// <summary>
    /// AutomationStudio Tree구조의 기본형
    /// </summary>
    public abstract class NodeBase : INode
    {
        [JsonIgnore]
        [Browsable(false)]
        public virtual string DisplayImage => "/Automation.PluginCore;component/Image/frown.png";

        string _name;
        public virtual string Name
        {
            get => string.IsNullOrEmpty(_name) ? this.GetType().Name : _name;
            set => SetProperty(ref _name, value);
        }
        [Browsable(false)]
        public Guid guid => Guid.Empty;

        [JsonIgnore]
        [Browsable(false)]
        public bool IsSelected { get; set; } = false;

        #region Tree
        [JsonIgnore]
        [Browsable(false)]
        public virtual string Path => this.Parent!=null? Parent.Path + "/" + this.Name : this.Name;
        [Browsable(false)]
        public NodeCollection Items { get; set; } = new NodeCollection();

        INode _parent;
        [JsonIgnore]
        [Browsable(false)]
        public INode Parent 
        {
            get => _parent; 
            set => SetProperty(ref _parent, value); 
        }
        public virtual INode FindNode(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return null;
            var segments = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (!string.Equals(this.Name, segments[0], StringComparison.OrdinalIgnoreCase))
                return null;
            if (segments.Length == 1)
                return this;
            var nextPath = string.Join("/", segments.Skip(1));
            foreach (var child in Items)
            {
                if (child is NodeBase node)
                {
                    var result = node.FindNode(nextPath);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }
        public void RemoveFromParent()
        {
            Parent?.Items.Remove(this);
        }

        public void AddChild(INode child)
        {
            child.RemoveFromParent();
            child.Parent = this;
            Items.Add(child);
        }
        #endregion

        #region Activate
        /// <summary>
        /// 저장 시 string으로 저장되고 런타임중 Node가 되는 객체들의 데이터 Load 이후 활성화 
        /// </summary>
        public virtual void Activate()
        {
            //Node 활성화

            //모든 자식 Node 활성화
            foreach(INode node in Items)
            {
                node.Activate();
            }
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected virtual bool SetProperty<T>(ref T member, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(member, value)) return false;
            member = value;
            NotifyPropertyChanged(propertyName);
            return true;
        }
        #endregion

        #region IDisposable
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
