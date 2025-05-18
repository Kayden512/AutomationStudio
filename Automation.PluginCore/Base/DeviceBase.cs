using Automation.PluginCore.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.PluginCore.Base
{
    /// <summary>
    /// 모든 장치의 기본형
    /// </summary>
    public abstract class DeviceBase : NodeBase, IDevice
    {
        public override string Icon => "M21.71 20.29L20.29 21.71A1 1 0 0 1 18.88 21.71L7 9.85A3.81 3.81 0 0 1 6 10A4 4 0 0 1 2.22 4.7L4.76 7.24L5.29 6.71L6.71 5.29L7.24 4.76L4.7 2.22A4 4 0 0 1 10 6A3.81 3.81 0 0 1 9.85 7L21.71 18.88A1 1 0 0 1 21.71 20.29M2.29 18.88A1 1 0 0 0 2.29 20.29L3.71 21.71A1 1 0 0 0 5.12 21.71L10.59 16.25L7.76 13.42M20 2L16 4V6L13.83 8.17L15.83 10.17L18 8H20L22 4Z";

        [Browsable(false)]
        public virtual List<Type> ActionMenu
        {
            get
            {
                string actionNameSpace = this.GetType().Namespace + ".Action";
                return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(asm =>
                {
                    try { return asm.GetTypes(); }
                    catch { return Array.Empty<Type>(); } // 리플렉션 예외 방지
                })
                .Where(t => typeof(IAction).IsAssignableFrom(t)
                            && !t.IsInterface
                            && !t.IsAbstract
                            && t.Namespace != null
                            && t.Namespace.StartsWith(actionNameSpace))
                .ToList();
            }
        }

        [Browsable(false)]
        public NodeCollection Actions { get; set; } = new NodeCollection();

        public override IEnumerable<IErrorItem> CollectErrors()
        {
            foreach (var error in Validate())
                yield return error;

            foreach (var child in Items)
            {
                foreach (var error in child.CollectErrors())
                    yield return error;
            }
            foreach (var child in Actions)
            {
                foreach (var error in child.CollectErrors())
                    yield return error;
            }
        }

        public void Connect()
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public DeviceBase() : base()
        {
            this.Actions.CollectionChanged += Items_CollectionChanged;
        }
    }
}
