using Automation.PluginCore.Util.Extension;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Automation.PluginCore.Util.Converter
{
    public class PathToGuidConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Guid id)
            {
                var action = Extension.Extension.GetNodeById(id);
                return action?.Path ?? "";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = value as string;
            var action = Extension.Extension.GetActions().FirstOrDefault(a => a.Path == path);
            return action?.Id ?? Guid.Empty;
        }
    }
}
