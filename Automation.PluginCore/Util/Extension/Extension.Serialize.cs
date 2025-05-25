using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.PluginCore.Util.Extension
{
    public static partial class Extension
    {
        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented
        };
        public static void SaveToJson<T>(string path, T data)
        {
            var json = JsonConvert.SerializeObject(data, _settings);
            File.WriteAllText(path, json);
        }
        public static T LoadFromJson<T>(string path)
        {
            if (!File.Exists(path))
                return default;
            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(json, _settings);
        }
    }
}
