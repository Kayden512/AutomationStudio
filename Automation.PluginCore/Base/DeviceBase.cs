using Automation.PluginCore.Interface;
using Automation.PluginCore.Util;
using Automation.PluginCore.Util.Extension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Automation.PluginCore.Base
{
    public enum ConnectionStatus { Connecting, Connected, Disconnected }
    /// <summary>
    /// 모든 장치의 기본형
    /// </summary>
    public abstract class DeviceBase : NodeBase, IDevice
    {
        ConnectionStatus _connectionStatus = ConnectionStatus.Disconnected;
        public override string Icon => "M21.71 20.29L20.29 21.71A1 1 0 0 1 18.88 21.71L7 9.85A3.81 3.81 0 0 1 6 10A4 4 0 0 1 2.22 4.7L4.76 7.24L5.29 6.71L6.71 5.29L7.24 4.76L4.7 2.22A4 4 0 0 1 10 6A3.81 3.81 0 0 1 9.85 7L21.71 18.88A1 1 0 0 1 21.71 20.29M2.29 18.88A1 1 0 0 0 2.29 20.29L3.71 21.71A1 1 0 0 0 5.12 21.71L10.59 16.25L7.76 13.42M20 2L16 4V6L13.83 8.17L15.83 10.17L18 8H20L22 4Z";

        CancellationTokenSource _cts;

        [JsonIgnore]
        [Browsable(false)]
        public ConnectionStatus ConnectionStatus
        {
            get => _connectionStatus;
            set => SetProperty(ref _connectionStatus, value);
        }

        [JsonIgnore]
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
                    catch { return Array.Empty<Type>(); }
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
        public async Task<object> ExecuteActionAsync(IAction action)
        {
            object result = null;
            try
            {
                Extension.AppendLog(ErrorSeverity.Info, $"{action.Name} Executeing");
                action.ActionStatus = ActionStatus.Running;
                _cts = new CancellationTokenSource();
                result = await action.ExecuteAsync(_cts.Token);
                action.ActionStatus = ActionStatus.Complete;
                return result;
            }
            catch (OperationCanceledException)
            {
                action.ActionStatus = ActionStatus.Stopped;
                throw;
            }
            catch (Exception)
            {
                action.ActionStatus = ActionStatus.Error;
                throw;
            }
            finally
            {

            }
        }

        public virtual void StopAction()
        {
            if(_cts != null && _cts.IsCancellationRequested == false)
            {
                _cts.Cancel();
            }
        }

        public virtual void Connect()
        {
            throw new NotImplementedException();
        }

        public virtual void Disconnect()
        {
            throw new NotImplementedException();
        }

        public virtual void Initialize()
        {
            throw new NotImplementedException();
        }
        public void TryConnect()
        {
            try
            {
                this.ConnectionStatus = ConnectionStatus.Connecting;
                Connect();
                this.ConnectionStatus = ConnectionStatus.Connected;
            }
            catch
            {
                this.ConnectionStatus = ConnectionStatus.Disconnected;
            }
        }

        public void TryDisconnect()
        {
            try
            {
                Disconnect();
                this.ConnectionStatus = ConnectionStatus.Disconnected;
            }
            catch
            {

            }
        }


        public virtual void AddAction(INode action)
        {
            action.RemoveFromParent();
            action.Parent = this;
            Extension.Register(action);
            this.Actions.Add(action);
        }

        public override INode FindNode(string path)
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
            foreach (var child in Actions)
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
        public override IEnumerable<IErrorItem> Validate()
        {
            if (this.ConnectionStatus == ConnectionStatus.Disconnected)
            {
                yield return new ErrorItem
                {
                    Severity = ErrorSeverity.Error,
                    Code = "NODE0001",
                    Message = "Device disconnected",
                    Node = this.Name,
                    Path = this.Path
                };
            }
            foreach (IErrorItem item in base.Validate())
            {
                yield return item;
            }
        }

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
        public DeviceBase() : base()
        {
            this.Actions.CollectionChanged += Items_CollectionChanged;
        }
    }
}
