using Automation.PluginCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.PluginCore.Util
{
    public static partial class Extension
    {
        public static IMain Main { get; set; }
        public static void OpenDocument(IDocument document)
        {
            Main.OpenDocument(document);
        }
        public static void CloseDocument(IDocument document)
        {
            Main.CloseDocument(document);
        }
    }
}
