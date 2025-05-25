using Automation.PluginCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.PluginCore.Util.Extension
{
    public static partial class Extension
    {
        public static Dictionary<Guid, INode> _nodes = new Dictionary<Guid, INode>();
        public static void Register(INode node)
        {
            if (!_nodes.ContainsKey(node.Id))
                _nodes[node.Id] = node;
        }
        public static void Unregister(INode node)
        {
            if (_nodes.ContainsKey(node.Id))
            {
                _nodes.Remove(node.Id);
            }
        }
        
        public static INode GetNodeById(Guid id)
        {
            return _nodes.TryGetValue(id, out var node) ? node : null;
        }

        public static List<INode> GetNodes(Type[] baseTypes = null)
        {
            List<INode> nodes = new List<INode>();
            foreach(var keyValue in _nodes)
            {
                if(baseTypes == null)
                {
                    nodes.Add(keyValue.Value);
                }
                else
                {
                    foreach (Type type in baseTypes)
                    {
                        if (type.IsAssignableFrom(keyValue.Value.GetType()))
                        {
                            nodes.Add(keyValue.Value);
                        }
                    }
                }
            }
            return nodes;
        }
    }
}
