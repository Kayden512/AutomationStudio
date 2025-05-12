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
    public static class PluginManager
    {
        public static List<Assembly> Assemblies = new List<Assembly>();
        public static List<Type> DeviceTypes = new List<Type>();
        public static void LoadPlugins(string folderPath)
        {
            if (!Directory.Exists(folderPath)) return;
            Assemblies.Add(Assembly.GetEntryAssembly());//Automation Studio Assembly 추가
            Assemblies.Add(Assembly.GetExecutingAssembly());//Automation.PluginCore Assembly 추가
            foreach (var dll in Directory.GetFiles(folderPath, "*.dll"))
            {
                try
                {
                    var assembly = Assembly.LoadFrom(dll);
                    Assemblies.Add(assembly);
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
            foreach (var assembly in Assemblies)
            {
                var types = assembly.GetTypes().Where(t => typeof(IDevice).IsAssignableFrom(t) && !t.IsAbstract);
                foreach (var type in types)
                    DeviceTypes.Add(type);
            }
        }
        public static List<Type> LoadType(Type target)
        {
            List<Type> typeLoaded = new List<Type>();
            foreach (var assembly in Assemblies)
            {
                var types = assembly.GetTypes().Where(t => target.IsAssignableFrom(t) && !t.IsAbstract);
                foreach (var type in types)
                    typeLoaded.Add(type);
            }
            return typeLoaded;
        }
    }
}