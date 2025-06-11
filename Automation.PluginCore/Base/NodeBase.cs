using Automation.PluginCore.Interface;
using Automation.PluginCore.Util;
using Automation.PluginCore.Util.Extension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Automation.PluginCore.Base
{
    /// <summary>
    /// AutomationStudio Tree구조의 기본형
    /// </summary>
    [CategoryOrder("Base",0)]
    public abstract class NodeBase : INode
    {
        string _name;
        INode _parent;

        [JsonIgnore]
        [Browsable(false)]
        public virtual string Icon => "M12,20A8,8 0 0,1 4,12A8,8 0 0,1 12,4A8,8 0 0,1 20,12A8,8 0 0,1 12,20M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12A10,10 0 0,0 12,2Z";

        [Category("Base")]
        public virtual string Name
        {
            get => string.IsNullOrEmpty(_name) ? this.GetType().Name : _name;
            set => SetProperty(ref _name, value);
        }
        [Browsable(false)]
        public Guid Id { get; set; } = Guid.NewGuid();


        [JsonIgnore]
        [Browsable(false)]
        public bool IsSelected { get; set; } = false;

        #region Tree
        [JsonIgnore]
        [Browsable(false)]
        public virtual string Path => this.Parent!=null? Parent.Path + "/" + this.Name : this.Name;

        [JsonIgnore]
        [Browsable(false)]
        public INode Parent 
        {
            get => _parent; 
            set => SetProperty(ref _parent, value);
        }
        [Browsable(false)]
        public NodeCollection Items { get; set; } = new NodeCollection();


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
        public virtual void RemoveFromParent()
        {
            Extension.Unregister(this);
            Parent?.Items.Remove(this);
        }

        public virtual void AddChild(INode child)
        {
            child.RemoveFromParent();
            child.Parent = this;
            Extension.Register(child);
            Items.Add(child);
        }
        #endregion

        #region Activate
        public virtual void Register()
        {
            //Node 활성화
            Extension.Register(this);

            //클래스 내 모든 속성 INode
            var nodeProperties = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                     .Where(p => typeof(INode).IsAssignableFrom(p.PropertyType));

            foreach (var prop in nodeProperties)
            {
                var node = prop.GetValue(this) as INode;
                if (node == this.Parent) continue;
                node?.Register();
            }

            ////클래스 내 모든 필드 NodeCollecion
            var nodeCollection = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                     .Where(p => typeof(NodeCollection).IsAssignableFrom(p.PropertyType));

            foreach (var prop in nodeCollection)
            {
                var collection = prop.GetValue(this) as NodeCollection;
                foreach (INode node in collection)
                    node?.Register();
            }
        }
        public virtual void Activate()
        {
            //클래스 내 모든 속성 INode
            var nodeProperties = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                     .Where(p => typeof(INode).IsAssignableFrom(p.PropertyType));

            foreach (var prop in nodeProperties)
            {
                var node = prop.GetValue(this) as INode;
                if (node == this.Parent) continue;
                node?.Activate();
            }

            ////클래스 내 모든 필드 NodeCollecion
            var nodeCollection = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                     .Where(p => typeof(NodeCollection).IsAssignableFrom(p.PropertyType));

            foreach (var prop in nodeCollection)
            {
                var collection = prop.GetValue(this) as NodeCollection;
                foreach (INode node in collection)
                    node?.Activate();
            }
        }
        public virtual IEnumerable<IErrorItem> Validate()
        {
            //경로 추적 불가 시 경고
            if (Parent == null)
            {
                yield return new ErrorItem
                {
                    Severity = ErrorSeverity.Warning,
                    Code = "NODE001",
                    Message = "ParentNode Missing",
                    Node = this.Name,
                    Path = this.Path
                };
            }
        }
        public virtual IEnumerable<IErrorItem> CollectErrors()
        {
            foreach (var error in Validate())
                yield return error;

            foreach (var child in Items)
            {
                foreach (var error in child.CollectErrors())
                    yield return error;
            }
        }

        public virtual void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.Action is System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach(INode node in e.NewItems)
                {
                    node.Parent = this;
                }
            }
        }
        #endregion

        public NodeBase()
        {
            Items.CollectionChanged += Items_CollectionChanged;
        }

        public override string ToString()
        {
            return this.Path;
        }

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

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            //Extension.Register(this);//역직렬화 이후 실행됨
        }

        #region IDisposable
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Extension.Unregister(this);
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
