using PluginLib;

namespace Plugin1;

[PluginLoad([typeof(Plugin2.Plugin2), typeof(Plugin3.Plugin3)])]
public class Plugin1 : IPlugin
{
    public void Execute()
    {
        Console.WriteLine("Plugin1 was executed");
    }
}
