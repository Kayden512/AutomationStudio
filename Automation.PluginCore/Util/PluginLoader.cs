using Automation.PluginCore.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Automation.PluginCore.Util
{
    public static class PluginLoader
    {
        public static List<IDevice> LoadPlugins(string folderPath)
        {
            var plugins = new List<IDevice>();

            if (!Directory.Exists(folderPath))
                return plugins;

            foreach (var dll in Directory.GetFiles(folderPath, "*.dll"))
            {
                try
                {
                    var assembly = Assembly.LoadFrom(dll);
                    var types = assembly.GetTypes()
                        .Where(t => typeof(IDevice).IsAssignableFrom(t) && !t.IsAbstract);

                    foreach (var type in types)
                    {
                        if (Activator.CreateInstance(type) is IDevice plugin)
                        {
                            plugins.Add(plugin);
                        }
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    // DLL 내 일부 타입이 실패해도 무시하고 가능한 것만 로드
                    foreach (var loaderException in ex.LoaderExceptions)
                        Console.WriteLine(loaderException.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[{dll}] Plugin Load Error: {ex.Message}");
                }
            }

            return plugins;
        }
    }
}