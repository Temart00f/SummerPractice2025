using System.IO;
using System.Reflection;
using PluginLib;

namespace task10;

public class PluginLoader
{
    private List<IPlugin> pluginsToLoad = [];

    public void LoadPlugins(string path)
    {
        var plugins = new List<(Type Type, PluginLoadAttribute Attr)>();

        try
        {
            string[] files = Directory.GetFiles(path, "*.dll");
            
            foreach (string file in files)
            {
                try
                {
                    var asm = Assembly.LoadFrom(file);
                    Type[] types = asm.GetTypes();

                    foreach (Type type in types)
                    {
                        var attr = type.GetCustomAttribute<PluginLoadAttribute>();

                        if (typeof(IPlugin).IsAssignableFrom(type) && attr is not null)
                        {
                            plugins.Add((type, attr));
                        }
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }
        catch (Exception)
        {
            throw new DirectoryNotFoundException("Path is uncorrect");
        }

        while (plugins.Count != 0)
        {
            int processedPlugins = 0;

            foreach (var (type, attr) in plugins.ToList())
            {
                if (attr.Dependencies.All(d => pluginsToLoad.Any(p => p.GetType() == d)))
                {
                    pluginsToLoad.Add((IPlugin)Activator.CreateInstance(type)!);
                    plugins.Remove((type, attr));
                    processedPlugins++;
                }
            }

            if (processedPlugins == 0)
            {
                break;   
            }
        }

        foreach (IPlugin plugin in pluginsToLoad)
        {
            plugin.Execute();
        }
    }
}

