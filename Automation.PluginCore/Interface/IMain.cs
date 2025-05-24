using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.PluginCore.Interface
{
    public interface IMain : INode
    {
        void OpenDocument(IDocument document);
        void CloseDocument(IDocument document);
        List<IAction> GetAction(Type[] baseTypes);
    }
}
