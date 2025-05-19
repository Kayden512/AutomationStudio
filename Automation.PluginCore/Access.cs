using Automation.PluginCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Automation.PluginCore
{
    public class Access
    {
        private static readonly Access _instance = new Access();
        public static Access Instance => _instance;

        public IMain Main { get; set; }

        public void OpenDocument(IDocument document)
        {
            Main.OpenDocument(document);
        }
        public void CloseDocument(IDocument document)
        {
            Main.CloseDocument(document);
        }
    }
}
