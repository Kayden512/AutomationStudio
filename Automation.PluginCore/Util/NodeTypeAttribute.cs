using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.PluginCore.Util
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class NodeTypeAttribute : Attribute
    {
        public Type[] IncludedTypes { get; }
        public Type[] ExcludedTypes { get; }
        public NodeTypeAttribute(Type type)
        {
            IncludedTypes = new Type[] { type };
        }
        public NodeTypeAttribute(Type[] types) 
        {
            IncludedTypes = types;
        }
        public NodeTypeAttribute(Type[] types, Type[] exTypes)
        {
            IncludedTypes = types;
            ExcludedTypes = exTypes;
        }
    }
}
